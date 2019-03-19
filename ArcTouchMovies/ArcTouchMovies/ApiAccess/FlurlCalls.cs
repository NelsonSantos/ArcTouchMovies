using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;

namespace ArcTouchMovies.ApiAccess
{
    public class FlurlCalls
    {
        private Dictionary<string, object> m_HeaderList = new Dictionary<string, object>();

        public FlurlCalls(string urlBase, int defaulTimeoutInSeconds = 30)
        {
            this.UrlBase = urlBase.EndsWith("/") ? urlBase.Substring(0, urlBase.Length - 1) : urlBase;
            this.DefaulTimeoutInSeconds = defaulTimeoutInSeconds;

            this.InitHttpClient();

        }
        private void InitHttpClient()
        {
            FlurlHttp.Configure(c =>
            {
                c.HttpClientFactory = new DependencieHttpClient();
                c.JsonSerializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings()
                {
                    Culture = CultureInfo.InvariantCulture
                });
            });
        }

        public void ClearHeaderList()
        {
            lock (m_HeaderList)
            {
                m_HeaderList = new Dictionary<string, object>();
            }
        }
        public void AddHeader(string name, object value)
        {
            lock (m_HeaderList)
            {
                //throw new Exception("Já existe um header com este nome na coleção. Por favor, verifique e tente novamente.");
                if (!m_HeaderList.ContainsKey(name))
                    m_HeaderList.Add(name, value);
            }
        }
        public string UrlBase { get; } = null;
        //public Auth AuthObject { get; private set; }
        public int DefaulTimeoutInSeconds { get; }

        private IFlurlRequest GetBaseWebService(string methodName, int defaulTimeoutinSeconds = 30)
        {
            var _methodName = methodName.ToLower().StartsWith("http") ? methodName : $"{(methodName.StartsWith("/") ? "" : "/")}{methodName}";
            var _url = "";

            if (string.IsNullOrEmpty(UrlBase))
                throw new Exception("URL base precisa ser informada!");

            _url = $"{this.UrlBase}{_methodName}";

            var _ret = _url.WithTimeout(defaulTimeoutinSeconds);

            if (UrlBase == null)
                _ret.WithHeader("version", 1);

            // varre a lista de header e os insere
            foreach (var _header in m_HeaderList)
            {
                _ret.WithHeader(_header.Key, _header.Value);
            }

            return _ret;
        }

        public async Task<ServiceResult<TResult>> GetAsync<TResult>(string methodName, int retryCount = 1, int secondsTimeout = 30, [CallerMemberName]string callerMemberName = null)
        {
            var _webApiObject = GetBaseWebService(methodName, secondsTimeout);
            var _resolved = await this.ResolveHttpResponseAsync<TResult>(DoType.Get, retryCount, _webApiObject, callerMemberName: callerMemberName);
            return _resolved;
        }
        public async Task<ServiceStatus> PostAsync(string methodName, object bodyObject, int retryCount = 1, int secondsTimeout = 30, [CallerMemberName]string callerMemberName = null)
        {
            var _status = await this.PostAsync<object>(methodName, bodyObject, retryCount, secondsTimeout, callerMemberName);
            return new ServiceStatus(_status.Success, _status.StatusCode, _status.StatusMessage);
        }
        public async Task<ServiceResult<TResult>> PostAsync<TResult>(string methodName, object bodyObject, int retryCount = 1, int secondsTimeout = 30, [CallerMemberName]string callerMemberName = null)
        {
            var _webApiObject = GetBaseWebService(methodName, secondsTimeout);
            var _resolved = await this.ResolveHttpResponseAsync<TResult>(DoType.Post, retryCount, _webApiObject, bodyObject, secondsTimeout, callerMemberName);
            return _resolved;
        }
        public async Task<bool> PutAsync(string methodName, object bodyObject, int retryCount = 1, int secondsTimeout = 30, [CallerMemberName]string callerMemberName = null)
        {
            var _ret = await this.PutAsync<object>(methodName, bodyObject, retryCount, secondsTimeout, callerMemberName);
            return _ret.Success;
        }
        public async Task<ServiceResult<TResult>> PutAsync<TResult>(string methodName, object bodyObject, int retryCount = 1, int secondsTimeout = 30, [CallerMemberName]string callerMemberName = null)
        {
            var _webApiObject = GetBaseWebService(methodName, secondsTimeout);
            var _resolved = await this.ResolveHttpResponseAsync<TResult>(DoType.Put, retryCount, _webApiObject, bodyObject, callerMemberName: callerMemberName);
            return _resolved;
        }
        public async Task<bool> DeleteAsync(string methodName, object bodyObject = null, int retryCount = 1, int secondsTimeout = 30, [CallerMemberName]string callerMemberName = null)
        {
            var _webApiObject = GetBaseWebService(methodName, secondsTimeout);
            var _resolved = await this.ResolveHttpResponseAsync<bool>(DoType.Delete, retryCount, _webApiObject, bodyObject, callerMemberName: callerMemberName);
            return _resolved.Result;
        }
        public async Task<ServiceResult<TResult>> DeleteAsync<TResult>(string methodName, object bodyObject = null, int retryCount = 1, int secondsTimeout = 30, [CallerMemberName]string callerMemberName = null)
        {
            var _webApiObject = GetBaseWebService(methodName, secondsTimeout);
            var _resolved = await this.ResolveHttpResponseAsync<TResult>(DoType.Delete, retryCount, _webApiObject, bodyObject, callerMemberName: callerMemberName);
            return _resolved;
        }

        private async Task<ServiceResult<TResult>> ResolveHttpResponseAsync<TResult>(DoType type, int retryCount, IFlurlRequest webApiObject, object bodyObject = null, int secondsTimeout = 30, string callerMemberName = null)
        {

            var _retMessage = "";
            ReturnMessage _message;
            HttpResponseMessage _exec = null;
            HttpStatusCode _HttpStatusCode = HttpStatusCode.BadRequest;

            var _policyResult = await Policy
                .Handle<FlurlHttpTimeoutException>().Or<FlurlHttpException>().Or<Exception>()
                .RetryAsync(retryCount, (exception, retryNumber) =>
                {
                    System.Diagnostics.Debug.WriteLine($"==================================================================");
                    System.Diagnostics.Debug.WriteLine($"Method: {callerMemberName} -> Retry: number#{retryNumber} -> exception:{exception.Message}");
                    System.Diagnostics.Debug.WriteLine($"==================================================================");
                })
                .ExecuteAndCaptureAsync(new Func<Task<ServiceResult<TResult>>>(async () =>
                {
                    var _token = this.GetCancelationToken(secondsTimeout).Token;
                    var _content = "";

#if DEBUG
                    var _json = JsonConvert.SerializeObject(bodyObject);
#endif
                    switch (type)
                    {
                        case DoType.Get:
                            _exec = await webApiObject.GetAsync(_token);
                            break;

                        case DoType.Post:
                            _exec = await webApiObject.PostJsonAsync(bodyObject, _token);
                            break;

                        case DoType.Put:
                            _exec = await webApiObject.PutJsonAsync(bodyObject, _token);
                            break;

                        case DoType.Delete:
                            _exec = bodyObject == null ? await webApiObject.DeleteAsync(_token) : await webApiObject.SendJsonAsync(HttpMethod.Delete, bodyObject, _token);
                            break;
                    }

                    switch (_exec.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            switch (type)
                            {
                                case DoType.Get:
                                case DoType.Post:
                                case DoType.Put:
                                    // lê o retorno do serviço
                                    var _stringContent = await _exec.Content.ReadAsStringAsync();

                                    // checa se está nulo, se estiver manda uma string vazia
                                    _stringContent = string.IsNullOrEmpty(_stringContent) ? "" : _stringContent;

                                    // vê se o conteúdo é um json, se não for, formatamos para que seja
                                    TResult _result = default(TResult);

                                    if (this.IsValidJson(_stringContent))
                                        _result = JsonConvert.DeserializeObject<TResult>(_stringContent);
                                    else
                                        _result = (TResult)Convert.ChangeType(_stringContent, typeof(TResult), CultureInfo.CurrentCulture);

                                    // retornamos o resultado deserializando no formato informado
                                    return new ServiceResult<TResult>(_result, true, _exec.StatusCode);

                                case DoType.Delete:
                                    object _RetBool = true;
                                    return new ServiceResult<TResult>((TResult)_RetBool, true, _exec.StatusCode);
                            }
                            break;

                        case HttpStatusCode.Unauthorized:
                            _retMessage = $"Você não está autorizado a executar essa rotina: {callerMemberName}!\r\nPor favor contacte o suporte.";
                            break;

                        case HttpStatusCode.InternalServerError:
                        case HttpStatusCode.BadRequest:
                            _content = await _exec.Content.ReadAsStringAsync();
                            try
                            {
                                _message = JsonConvert.DeserializeObject<ReturnMessage>(_content);
                                _retMessage = _message.Message;
                            }
                            catch (Exception)
                            {
                                _retMessage = _content;
                            }

                            break;

                        case HttpStatusCode.NoContent:
                            return new ServiceResult<TResult>(default(TResult), false, _exec.StatusCode);

                        case HttpStatusCode.NotFound:
                            return new ServiceResult<TResult>(default(TResult), false, _exec.StatusCode);

                        default:
                            _content = await _exec.Content.ReadAsStringAsync();
                            _message = JsonConvert.DeserializeObject<ReturnMessage>(_content);
                            _retMessage = $"Ocorreu um erro ao executar operação na rotina {callerMemberName}!\r\nPor favor contacte o suporte:\r\n\r\n{_message.Message}";
                            break;
                    }
                    _HttpStatusCode = _exec.StatusCode;
                    return new ServiceResult<TResult>(default(TResult), false, _HttpStatusCode, _retMessage);
                }));

            if (_policyResult.Outcome == OutcomeType.Successful)
            {
                return _policyResult.Result;
            }
            else
            {
                switch (_policyResult.FinalException)
                {
                    case FlurlHttpTimeoutException fhtex:
                        _HttpStatusCode = HttpStatusCode.GatewayTimeout;
                        _retMessage = $"O tempo para execução desta tarefa expirou!Rotina{callerMemberName}\r\nTente novamente em alguns segundos.";
                        break;

                    case FlurlHttpException fhex:
                        string _retString = fhex.Message;
                        if (fhex.Call.HttpStatus.HasValue)
                        {
                            _HttpStatusCode = fhex.Call.HttpStatus.Value;
                            switch (fhex.Call.HttpStatus.Value)
                            {
                                case HttpStatusCode.InternalServerError:
                                case HttpStatusCode.BadRequest:
                                    _retMessage = fhex.Call.Response.Content.ReadAsStringAsync().Result;
                                    break;
                            }
                        }
                        else
                        {
                            //_retMessage = $"Ocorreu um erro ao executar a operação na rotina {callerMemberName}!\r\nPor favor contacte o suporte:\r\n\r\n{_retString}";
                            _retMessage = $"Não foi possível executar a sua solicitação. Verifique a sua conexão com a Internet e tente novamente.";
                        }
                        break;

                    case Exception ex:
                        _retMessage = this.GetInnerExceptionMessages(ex);
                        break;

                    default:
                        _retMessage = this.GetInnerExceptionMessages(_policyResult.FinalException);
                        break;
                }
                return new ServiceResult<TResult>(default(TResult), false, _HttpStatusCode, _retMessage);
            }
        }

        private string GetInnerExceptionMessages(Exception ex)
        {
            var _ret = new StringBuilder();

            _ret.AppendLine(ex.Message);

            if (ex.InnerException != null)
                _ret.Append(this.GetInnerExceptionMessages(ex.InnerException));

            return _ret.ToString();
        }

        private CancellationTokenSource GetCancelationToken(int secondsTimeout = 300)
        {
            var _time = TimeSpan.FromSeconds(secondsTimeout);
            var _cancelToken = new CancellationTokenSource(_time);
            _cancelToken.CancelAfter(_time);
            return _cancelToken;
        }

        private bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public enum DoType
        {
            Get,
            Post,
            Put,
            Delete
        }
    }
}
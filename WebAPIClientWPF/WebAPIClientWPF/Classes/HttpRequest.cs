using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using WebAPIClientWPF.Windows;

namespace WebAPIClientWPF.Classes
{
    internal class HttpRequest
    {   
        private const string _url = "https://localhost:7039/api/UserItems";
        private const string _contentType = "application/json";
        private const string _methodPost = "POST";
        private const string _methodPut = "PUT";
        private const string _methodDelete = "DELETE";
        private const string _addressLogin = "Login";
        private const string _addressCreate = "Create";
        private static ResponseWaiting _responseWaiting;
    
        public static async Task<User> GetUser(string login)
        {
            ShowResponceWaitingWindow();
            var request = (HttpWebRequest)WebRequest.Create($"{_url}/{login}");
            try
            {
                var response = await GetResponse(request);
                return JsonConvert.DeserializeObject<User>(response);
            }
            catch (WebException exception)
            {
                ShowRequestError(exception);
                HideResponceWaitingWindow();
                return new User();
            }
        }

        public static async Task<bool> TryLogin(string login, string password)
        {
            ShowResponceWaitingWindow();
            var request = (HttpWebRequest)WebRequest.Create($"{_url}/{_addressLogin}?{nameof(login)}={login}&{nameof(password)}={password}");
            request.Method = _methodPost;
            try
            {
                var response = await GetResponse(request);
                return Convert.ToBoolean(response);
            }
            catch (WebException exception)
            {
                ShowRequestError(exception);
                HideResponceWaitingWindow();
                return false;
            }
        }

        public static async Task<bool> AddUser(string login, string password, string nickname)
        {
            ShowResponceWaitingWindow();
            var request = (HttpWebRequest)WebRequest.Create($"{_url}/{_addressCreate}");
            request.ContentType = _contentType;
            request.Method = _methodPost;
            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(new User()
                    {
                        login = login,
                        password = password,
                        nickname = nickname
                    });
                    streamWriter.Write(json);
                }
                var response = (HttpWebResponse)await request.GetResponseAsync();
                HideResponceWaitingWindow();
                return response.StatusCode == HttpStatusCode.Created;
            }
            catch (WebException exception)
            {
                ShowRequestError(exception);
                HideResponceWaitingWindow();
                return false;
            }
        }

        public static async Task<bool> UpdatePassword(string login, string password)
        {
            ShowResponceWaitingWindow();
            var request = (HttpWebRequest)WebRequest.Create($"{_url}/{login}/{nameof(password)}?{nameof(password)}={password}");
            return await GetResponse(request, _methodPut);
        }

        public static async Task<bool> UpdateNickname(string login, string nickname)
        {
            ShowResponceWaitingWindow();
            var request = (HttpWebRequest)WebRequest.Create($"{_url}/{login}/{nameof(nickname)}?{nameof(nickname)}={nickname}");
            return await GetResponse(request, _methodPut);
        }

        public static async Task<bool> DeleteRequest(string login)
        {
            ShowResponceWaitingWindow();
            var request = (HttpWebRequest)WebRequest.Create($"{_url}/{login}");
            return await GetResponse(request, _methodDelete);
        }

        private static async Task<string> GetResponse(HttpWebRequest request)
        {
            var response = (HttpWebResponse)await request.GetResponseAsync();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string value = reader.ReadToEnd();
                response.Close();
                HideResponceWaitingWindow();
                return value;
            }
        }

        private static async Task<bool> GetResponse(HttpWebRequest request, string method)
        {
            request.Method = method;
            try
            {
                var response = (HttpWebResponse)await request.GetResponseAsync();
                HideResponceWaitingWindow();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (WebException exception)
            {
                ShowRequestError(exception);
                HideResponceWaitingWindow();
                return false;
            }
        }

        private static void ShowRequestError(WebException exception)
        {
            if ((exception.Status == WebExceptionStatus.ConnectFailure) || (exception.Status == WebExceptionStatus.ProtocolError))
                MessageBox.Show($"Request error: {exception.Message}");
        }

        private static void ShowResponceWaitingWindow()
        {
            _responseWaiting = new ResponseWaiting();
            _responseWaiting.Show();
        }

        private static void HideResponceWaitingWindow()
        {
            if (_responseWaiting != null)
                _responseWaiting.Close();
        }
    }
}

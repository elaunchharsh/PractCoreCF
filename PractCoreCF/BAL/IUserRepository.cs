using PractCoreCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PractCoreCF.BAL
{
    public interface IUserRepository:IDisposable
    {
        public UserMaster AddUpdateUserMaster(UserMaster user);
        public UserMaster GetLogin(string email, string password);
        public bool DeleteUser(int UserId);
        public List<UserMaster> GetAllUsers();
        public UserMaster GetUserByUserId(int UserId);

        public List<CountryMaster> GetAllCountries();
        public CountryMaster GetCountryById(int CountryId);

        public List<StateMaster> GetStateListByCountryId(int CountryId);
        public StateMaster GetStateById(int StateId);

        public bool AddUpdateTokenMaster(TokenMaster token);

        public string GetTokenByUserId(int UserId);

        public bool GetAuthentication(string token);

        public int GetUserIdFromToken(string token);

        public UserMaster getUserInformationByToken(string token);

        public bool DeleteUserImage(int imageId);

        public List<UserImages> GetUserImagesByUserId(int UserId);

        public bool DeleteAllImagesByUserId(int UserId);

        public bool SaveUserImage(UserImages image);

        public bool SaveMultipleUserImages(List<UserImages> userImages);

    }
}

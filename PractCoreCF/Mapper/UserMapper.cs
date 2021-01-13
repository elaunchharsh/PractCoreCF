using PractCoreCF.Models;
using PractCoreCF.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PractCoreCF.Mapper
{
    public static class UserMapper
    {

        #region ModelView TO DB Mappers
        public static UserMaster MapSingleUserToUserMaster(UserMasterView userMaster)
        {
            try
            {
                if (userMaster != null)
                {
                    UserMaster l_mappedUser = new UserMaster()
                    {
                        Address = userMaster.Address,
                        ContactNumber = userMaster.ContactNumber,
                        CreateDate = userMaster.CreateDate,
                        Email = userMaster.Email,
                        FirstName = userMaster.FirstName,
                        Gender = userMaster.Gender,
                        Hobbies = string.Join(",", userMaster.Hobbies),
                        LastName = userMaster.LastName,
                        Password = userMaster.Password,
                        PostCode = userMaster.PostCode,
                        UpdateDate = userMaster.UpdateDate,
                        UserId = userMaster.UserId,
                        UserImages = MapMultipleImagesToImageMaster(userMaster.UserImages),
                        CountryMaster = MapSingleCountryToCountryMaster(userMaster.CountryMaster),
                        StateMaster=MapSingleStateToStateMaster(userMaster.StateMaster),
                        CountryId=userMaster.CountryId,
                        StateId=userMaster.StateId
                    };
                    return l_mappedUser;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static UserImages MapSingleImageToImageMaster(UserImagesView imagesView)
        {
            try
            {
                if (imagesView != null)
                {
                    UserImages mappedImage = new UserImages()
                    {
                        FileName = imagesView.FileName,
                        UserId = imagesView.UserId,
                        UserImageId = imagesView.UserImageId
                    };
                    return mappedImage;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<UserImages> MapMultipleImagesToImageMaster(List<UserImagesView> LstImagesView)
        {
            try
            {
                List<UserImages> userImages = new List<UserImages>();
                if (LstImagesView != null && LstImagesView.Count > 0)
                {
                    foreach (var item in LstImagesView)
                    {
                        userImages.Add(MapSingleImageToImageMaster(item));
                    }
                }
                return userImages;
            }
            catch (Exception ex)
            {
                return new List<UserImages>();
            }
        }

        public static CountryMaster MapSingleCountryToCountryMaster(CountryMasterView countryView)
        {
            try
            {
                if (countryView != null)
                {
                    CountryMaster country = new CountryMaster()
                    {
                        CountryId = countryView.CountryId,
                        CountryName = countryView.CountryName
                    };
                    return country;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<CountryMaster> MapMultipleCountryToCountryMaster(List<CountryMasterView> countries)
        {
            try
            {
                List<CountryMaster> countyList = new List<CountryMaster>();
                if (countries != null && countries.Count > 0)
                {
                    foreach (var item in countries)
                    {
                        countyList.Add(MapSingleCountryToCountryMaster(item));
                    }
                }
                return countyList;
            }
            catch (Exception ex)
            {
                return new List<CountryMaster>();
            }
        }

        public static StateMaster MapSingleStateToStateMaster(StateMasterView stateMasterView)
        {
            try
            {
                if (stateMasterView != null)
                {
                    StateMaster stateMaster = new StateMaster()
                    {
                        CountryId = stateMasterView.CountryId,
                        StateId = stateMasterView.StateId,
                        StateName = stateMasterView.StateName,
                        CountryMaster = MapSingleCountryToCountryMaster(stateMasterView.CountryMaster)
                    };
                    return stateMaster;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<StateMaster> MapMultipleStateToStateMaster(List<StateMasterView> stateMasterViews)
        {
            try
            {
                List<StateMaster> stateMasters = new List<StateMaster>();
                if (stateMasterViews != null && stateMasterViews.Count > 0)
                {
                    foreach (var item in stateMasterViews)
                    {
                        stateMasters.Add(MapSingleStateToStateMaster(item));
                    }
                }
                return stateMasters;
            }
            catch (Exception ex)
            {
                return new List<StateMaster>();
            }
        }

        #endregion

        #region DB Class to ModelView Mappers
        public static UserMasterView MapSingleUserToUserMasterView(UserMaster userMaster)
        {
            try
            {
                if (userMaster != null)
                {
                    return new UserMasterView()
                    {
                        Address = userMaster.Address,
                        ContactNumber = userMaster.ContactNumber,
                        CountryMaster = UserMapper.MapSingleCountryToCountryMasterView(userMaster.CountryMaster),
                        Email = userMaster.Email,
                        FirstName = userMaster.FirstName,
                        Gender = userMaster.Gender,
                        Hobbies = userMaster.Hobbies.Split(',').ToList(),
                        LastName = userMaster.LastName,
                        Password = userMaster.Password,
                        PostCode = userMaster.PostCode,
                        StateMaster = UserMapper.MapSingleStateToStateMasterView(userMaster.StateMaster),
                        UpdateDate = userMaster.UpdateDate,
                        CreateDate = userMaster.CreateDate,
                        UserId = userMaster.UserId,
                        UserImages = UserMapper.MapMultipleImageToImageMasterView(userMaster.UserImages)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<UserMasterView> MapMultipleUserToUserMasterView(List<UserMaster> userMasters)
        {
            try
            {
                List<UserMasterView> userMasterViews = new List<UserMasterView>();
                if (userMasters != null && userMasters.Count > 0)
                {
                    foreach (var user in userMasters)
                        userMasterViews.Add(MapSingleUserToUserMasterView(user));
                }
                return userMasterViews;
            }
            catch (Exception ex)
            {
                return new List<UserMasterView>();
            }
        }
     
        public static UserImagesView MapSingleImageToImageMasterView(UserImages userImage)
        {
            try
            {
                if (userImage != null)
                {
                    UserImagesView userImagesView = new UserImagesView()
                    {
                        FileName = userImage.FileName,
                        UserId = userImage.UserId,
                        UserImageId = userImage.UserImageId
                    };
                    return userImagesView;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static List<UserImagesView> MapMultipleImageToImageMasterView(List<UserImages> userImages)
        {
            try
            {
                List<UserImagesView> l_userImages = new List<UserImagesView>();
                if (userImages != null && userImages.Count > 0)
                {
                    foreach (var item in userImages)
                    {
                        l_userImages.Add(MapSingleImageToImageMasterView(item));
                    }
                }
                return l_userImages;
            }
            catch (Exception ex)
            {
                return new List<UserImagesView>();
            }
        }

        public static CountryMasterView MapSingleCountryToCountryMasterView(CountryMaster countryMaster)
        {
            try
            {
                if (countryMaster != null)
                {
                    CountryMasterView countryMasterView = new CountryMasterView()
                    {
                        CountryId = countryMaster.CountryId,
                        CountryName = countryMaster.CountryName
                    };
                    return countryMasterView;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<CountryMasterView> MapMultipleCountryToCountryMasterView(List<CountryMaster> countryMasters)
        {
            try
            {
                List<CountryMasterView> l_CountryMasterView = new List<CountryMasterView>();
                if (countryMasters != null && countryMasters.Count > 0)
                {
                    foreach (var item in countryMasters)
                    {
                        l_CountryMasterView.Add(MapSingleCountryToCountryMasterView(item));
                    }
                }
                return l_CountryMasterView;

            }
            catch (Exception ex)
            {
                return new List<CountryMasterView>();
            }
        }

        public static StateMasterView MapSingleStateToStateMasterView(StateMaster stateMaster)
        {
            try
            {
                if (stateMaster != null)
                {
                    StateMasterView stateMasterView = new StateMasterView()
                    {
                        CountryId = stateMaster.CountryId,
                        CountryMaster = MapSingleCountryToCountryMasterView(stateMaster.CountryMaster),
                        StateId = stateMaster.StateId,
                        StateName = stateMaster.StateName
                    };
                    return stateMasterView;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static List<StateMasterView> MapMultipleStateToStateMasterView(List<StateMaster> stateMasters)
        {
            try
            {
                List<StateMasterView> stateMasterView = new List<StateMasterView>();
                if (stateMasters != null && stateMasters.Count > 0)
                {
                    foreach (var item in stateMasters)
                    {
                        stateMasterView.Add(MapSingleStateToStateMasterView(item));
                    }
                }
                return stateMasterView;
            }
            catch (Exception ex)
            {
                return new List<StateMasterView>();
            }
        }
        #endregion

        public static string HobbyIdsToString(List<string> ids)
        {
            var l_hbs = getAllHobbies().Where(x => ids.Contains(x.HobbyId.ToString())).Select(y=>y.HobbyName).ToList();
            return string.Join(",", l_hbs);
        }

        public static string GenderIdToString(int gender)
        {
            if (gender == 1)
                return "Male";
            else if (gender == 2)
                return "Female";
            else
                return "NA";
        }

        public static List<Hobbies> getAllHobbies()
        {
            List<Hobbies> hobbies = new List<Hobbies>();
            hobbies.Add(new Hobbies() { HobbyId = 1, HobbyName = "Coding" });
            hobbies.Add(new Hobbies() { HobbyId = 2, HobbyName = "Reading" });
            hobbies.Add(new Hobbies() { HobbyId = 3, HobbyName = "Travelling" });
            return hobbies;
        }
    }
}

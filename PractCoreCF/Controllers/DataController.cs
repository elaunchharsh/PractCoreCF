using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PractCoreCF.BAL;
using PractCoreCF.Models;


namespace PractCoreCF.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        IUserRepository _userRepository;
       
        public DataController(IConfiguration config, ApplicationDBContext dBContext, IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        [Route("api/Data/GetCountries")]
        [HttpGet]
        public ActionResult getAllCountries()
        {
            var l_ctrs=_userRepository.GetAllCountries();
            return Ok(l_ctrs);
        }

        [HttpGet]
        [Route("api/Data/GetStatesByCountryId/{countryId}")]
        public ActionResult getAllStatesByCountryId(int countryId)
        {
            var l_states = _userRepository.GetStateListByCountryId(countryId);
            return Ok(l_states);
        }
    }
}

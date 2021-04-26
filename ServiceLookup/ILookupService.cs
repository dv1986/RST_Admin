using ModelLookup;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLookup
{
    public interface ILookupService
    {
        #region Country
        bool AddCountry(Country country);
        IList<Country> UpdateCountry(List<Country> tasks);
        IList<Country> DeleteCountry(List<Country> tasks);
        List<Country> GetCountry(string SearchStr);
        List<Country> GetCountryLookup(string SearchStr);
        #endregion

        #region State
        bool AddState(State state);
        IList<State> UpdateState(List<State> tasks);
        IList<State> DeleteState(List<State> tasks);
        List<State> GetState(string SearchStr);
        List<State> GetStateLookup(int CountryId);
        #endregion

        #region City
        bool AddCity(City city);
        IList<City> UpdateCity(List<City> tasks);
        IList<City> DeleteCity(List<City> tasks);
        List<City> GetCityLookup(int StateId);
        List<City> GetCity(int CountryId, int StateId);
        #endregion

        #region Menu
        bool AddMenu(Menu menu);
        IList<Menu> UpdateMenu(List<Menu> tasks);
        IList<Menu> DeleteMenu(List<Menu> tasks);
        List<Menu> GetMenu(string SearchStr);
        #endregion

        #region Sub-Menu
        bool AddSubMenu(SubMenu subMenu);
        IList<SubMenu> UpdateSubMenu(List<SubMenu> tasks);
        IList<SubMenu> DeleteSubMenu(List<SubMenu> tasks);
        List<SubMenu> GetSubMenu(string SearchStr);
        #endregion

        #region Subscription
        List<Subscription> GetSubscriptionList();
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

using Mobimarket.BL;

namespace Mobimarket.Models
{
  public partial class Enterprise
  {
    private Income CurrentIncome { get; set; }

    public List<IncomeModel> yearIncomes { get; private set; }

    private Dictionary<int, List<IncomeModel>> monthIncomes { get; set; }

    private void LoadIncomes()
    {
      int minYear = Incomes.Select(income => income.Date.Year).Min();
      int maxYear = Incomes.Select(income => income.Date.Year).Max();

      for (int i = minYear; i <= maxYear; i++)
      {
        decimal yearIncome = 0;
        monthIncomes[i] = new List<IncomeModel>();
        for (int j = 1; j <= 12; j++)
        {
          var month = Incomes
            .Where(income => income.Date.Year == i && income.Date.Month == j)
            .Select(income => income.Money)
            .Sum();

          monthIncomes[i].Add(new IncomeModel
          {
            Date = j,
            Income = month
          });

          yearIncome += month;
        }

        yearIncomes.Add(new IncomeModel
        {
          Date = i,
          Income = yearIncome
        });
      }
    }

    public void UpdateIncome()
    {
      var month = CurrentIncome.Date.Month;
      var year = CurrentIncome.Date.Year;

      var date = DateTime.Now;
      if (date.Day != 1)
        return;

      CurrentIncome = new Income
      {
        Date = DateTime.Now,
        EnterpriseId = Id
      };

      CurrentIncome = Manager.mobiMarket.Incomes.Add(CurrentIncome);

      var monthIncome = Incomes
        .Where(income => income.Date.Month == month && income.Date.Year == year)
        .Union(new[] { CurrentIncome })
        .Select(x => x.Money)
        .Sum();

      monthIncomes[year].Add(new IncomeModel
      {
        Date = month,
        Income = monthIncome
      });

      if (date.Month != 1)
        return;

      var yearIncome = Incomes.Where(income => income.Date.Year == year)
        .Select(x => x.Money)
        .Sum();

      yearIncomes.Add(new IncomeModel
      {
        Date = year,
        Income = yearIncome
      });
    }

    public void AddIncome(decimal money)
    {
      CurrentIncome.Money += money;
      Manager.mobiMarket.SaveChanges();
    }

    public List<IncomeModel> GetAllTimeIncomes()
    {
      return yearIncomes;
    }

    public List<IncomeModel> GetYearIncome(int year)
    {
      return monthIncomes[year];
    }

    public List<Income> GetMonthIncomes(int year, int month)
    {
      return Incomes.Where(income => income.Date.Year == year && income.Date.Month == month).ToList();
    }
  }
}
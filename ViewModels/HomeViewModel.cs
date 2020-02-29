using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PieShop.Models;

namespace PieShop.ViewModels
{
    public class HomeViewModel
    {
	    public IEnumerable<Pie> PiesOfTheWeek { get; set; }
    }
}

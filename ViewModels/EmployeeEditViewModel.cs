using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement1.ViewModels
{
    public class EmployeeEditViewModel :EmployeeCreateViewModel
    {
        public int Id { get; set; }

        public string ExistingPhotoPath { get; set; }

        public string photoSelect { get; set; }


    }
}

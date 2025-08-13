using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMovies.ViewModels
{
    public class BaseVM
    {
        private static BaseVM baseInstance;
        public static BaseVM BaseInstance
        {
            get
            {
                if (baseInstance == null)
                {
                    baseInstance = new BaseVM();
                }
                return baseInstance;
            }
        }

        public BaseVM()
        {
            baseInstance = this;
        }
    }
}

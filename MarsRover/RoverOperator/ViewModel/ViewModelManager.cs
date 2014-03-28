using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverOperator
{
    public sealed class ViewModelManager
    {
        private static object syncRoot = new Object();

        private static volatile ViewModelManager instance;
        public static ViewModelManager Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ViewModelManager();
                        }
                    }
                }

                return instance;
            }
        }

        public MainWindowViewModel MainWindowVM { get; set; }

        private ViewModelManager()
        {

        }
    }
}

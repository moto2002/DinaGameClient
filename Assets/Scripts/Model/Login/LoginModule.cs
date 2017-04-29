using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Module.View;

namespace Assets.Scripts.Module
{
    class LoginModule
    {
        private LoginView loginView;

        public void Init()
        { 
            
        }
        public void OpenLoginView()
        {
            if (loginView == null)
            {
                loginView = new LoginView();
            }
        }

        private static LoginModule instance;
        public static LoginModule GetInstance()
        {
            if (instance == null)
            {
                instance = new LoginModule();
            }
            return instance;
        }
    }
}

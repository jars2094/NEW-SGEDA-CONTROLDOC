﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDoc.Services.Services
{
    public class AuthenticationStateContainer
    {
        #region Propiedades 
        public string? SelectedComponent { get; private set; } = "Login";
        public string? User { get; private set; }
        public string? Uuid { get; private set; }
        public string? Ip { get; private set; }

        #endregion

        #region Atributos
        public event Action ComponentChange;
        #endregion

        #region Metodos
        public void SelectedComponentChanged(string Component)
        {
            SelectedComponent = Component;
            ExecuteAction();
        }

        public void Parametros(string user, string uuid, string ip)
        {
            User = user; Uuid = uuid; Ip = ip;
            ExecuteAction();
        }

        private void ExecuteAction() => ComponentChange?.Invoke();  
        #endregion
    }
}

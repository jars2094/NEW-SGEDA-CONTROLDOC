using ControlDoc.Models.Enums;

namespace ControlDoc.Services.Interfaces
{
    public interface ISessionStorage
    {
        /// <summary>
        /// Obtiene un valor que se encuentra en la memoria local
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetValue<T>(ValuesKeys key);

        /// <summary>
        /// Obtiene un valor que se encuentra almacenado en memoria y lo serializa a un tipo de dato especifico 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetValue<T>(ValuesKeys key, T value);

        /// <summary>
        /// Ontiene la llaves especificas
        /// </summary>
        /// <param name="key">Tipo de llave</param>
        /// <returns></returns>
        Task<bool> ContainsKey(ValuesKeys key);

        /// <summary>
        /// Remueve de memoria un elemento en especifico
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveItem(ValuesKeys key);

        /// <summary>
        /// Elimina en memoria todos lo datos que estan guardados
        /// </summary>
        /// <returns></returns>
        Task ClearAll();
    }
}

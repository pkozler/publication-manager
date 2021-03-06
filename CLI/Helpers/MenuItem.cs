﻿namespace CLI
{
    /// <summary>
    /// Metoda třídy, která je součástí uživatelského rozhraní
    /// a zavolá se při použití některého z příkazů.
    /// </summary>
    public delegate void UserInterfaceMethod();

    /// <summary>
    /// Třída slouží k uchování informace o položce menu (příkazu) uživatelského rozhraní
    /// (zahrnuje název, popis a delegát na metodu volanou při zadání daného příkazu).
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Jednoslovný název příkazu, který slouží jako mnemotechnická pomůcka.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Jednořádkový text, který popisuje chování příkazu.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Metoda, která se zavolá při použití příkazu.
        /// </summary>
        public UserInterfaceMethod UIMethod { get; set; }
    }
}

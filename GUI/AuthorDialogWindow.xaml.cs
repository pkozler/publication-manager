﻿using System.Windows;
using Core;

namespace GUI
{
    /// <summary>
    /// Tato třída obsahuje interakční logiku pro AuthorDialogWindow.xaml
    /// představující dialogové okno, které slouží k zadání osobních údajů
    /// nového (zatím neevidovaného) autora zobrazené publikace.
    /// </summary>
    public partial class AuthorDialogWindow : Window
    {
        /// <summary>
        /// Uchovává zadané osobní údaje autora pro formulář publikace.
        /// </summary>
        public Author NewAuthor { get; private set; }

        /// <summary>
        /// Provede inicializaci komponent.
        /// </summary>
        public AuthorDialogWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Potvrdí zadání osobních údajů nového autora pro přidání do seznamu.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text)
                || string.IsNullOrWhiteSpace(surnameTextBox.Text))
            {
                return;
            }

            // vytvoření údajů nového autora
            NewAuthor = new Author();
            NewAuthor.Name = nameTextBox.Text;
            NewAuthor.Surname = surnameTextBox.Text;

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Zruší zadávání osobních údajů nového autora.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Ověří, zda bylo vyplněno pole pro jméno i pro příjmení v případě změny obsahu některého
        /// z těchto polí, a jsou-li obě pole vyplněna, aktivuje tlačítko pro potvrzení vytvoření autora
        /// s uvedenými osobními údaji (v opačném případě je tlačítko opět deaktivováno).
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void textBoxes_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            okButton.IsEnabled = !string.IsNullOrWhiteSpace(nameTextBox.Text)
                && !string.IsNullOrWhiteSpace(surnameTextBox.Text);
        }
    }
}

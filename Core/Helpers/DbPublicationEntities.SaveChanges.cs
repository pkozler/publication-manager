using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Rozšíření třídy DbPublicationEntities slouží k vylepšení chybové zprávy
    /// při vyhození výjimky, tak aby zpráva obsahovala popis chyb, které se vyskytly při validaci.
    /// </summary>
    public partial class DbPublicationEntities : DbContext
    {
        /// <summary>
        /// Provede uložení změn v databázi a v případě chyby vyhodí výjimku
        /// s vylepšenou chybovou zprávou.
        /// </summary>
        /// <returns>počet zapsaných obektů</returns>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // výběr chybových zpráv jako seznamu řetězců
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // spojení seznamu do jednoho řetězce
                var fullErrorMessage = string.Join("; ", errorMessages);

                // spojení původní chybové zprávy s nově vytvořenou
                var exceptionMessage = string.Concat(ex.Message, 
                    " Při vylidaci se vyskytly následující chyby: ", fullErrorMessage);

                // vyhození nové výjimky typu DbEntityValidationException s vylepšenou chybovou zprávou
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}

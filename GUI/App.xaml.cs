using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using Core;

namespace GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DIRECTORY = "../../resources/example-texts/";

        private const string EXTENSION = ".txt";

        /// <summary>
        /// Provede inicializaci objektů datové vrstvy.
        /// </summary>
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            try
            {
                // vytvoření databázového kontextu
                DbPublicationEntities context = new DbPublicationEntities();

                // vytvoření objektů pro správu dat společných pro všechny typy publikací
                PublicationModel publicationModel = new PublicationModel(context);
                AuthorModel authorModel = new AuthorModel(context);
                AttachmentModel attachmentModel = new AttachmentModel(context);

                // vytvoření seznamu typů publikací s přidruženými dialogy a objekty pro správu dat
                List<PublicationType> publicationTypes = initializeTypes(context);
                
                if (args.Length > 1 && args[1] == "-t")
                {
                    initializeData(authorModel, attachmentModel,
                        publicationTypes[0].Model,
                        publicationTypes[1].Model,
                        publicationTypes[2].Model,
                        publicationTypes[3].Model);
                }

                new MainWindow(publicationModel, authorModel, 
                    attachmentModel, publicationTypes).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba při inicializaci");
                Current.Shutdown(-1);
            }
        }

        /// <summary>
        /// Inicializuje objekty pro správu jednotlivých dostupných typů publikací.
        /// </summary>
        /// <param name="context">databázový kontext</param>
        /// <returns>seznam typů publikací s inicializovanými objekty pro jejich správu</returns>
        private static List<PublicationType> initializeTypes(DbPublicationEntities context)
        {
            // vytvoření objektů pro správu dat specifických pro jednotlivé dostupné typy publikací
            ConferenceArticleModel conferenceArticleModel = new ConferenceArticleModel(context, "Článek na konferenci");
            JournalArticleModel journalArticleModel = new JournalArticleModel(context, "Článek do časopisu");
            TechnicalReportModel technicalReportModel = new TechnicalReportModel(context, "Technická zpráva");
            QualificationThesisModel qualificationThesisModel = new QualificationThesisModel(context, "Kvalifikační práce");

            /*
                přiřazení dialogů uživatelského rozhraní k jednotlivým dostupným typům publikací,
                propojení s příslušnými objekty datové vrstvy a uložení typů publikací do seznamu
            */
            List<PublicationType> publicationTypes = new List<PublicationType>()
            {
                new PublicationType(ConferenceArticleModel.NAME, conferenceArticleModel,
                    (model) => {
                        return new ConferenceArticleUserControl(model);
                    }),
                new PublicationType(JournalArticleModel.NAME, journalArticleModel,
                    (model) => {
                        return new JournalArticleUserControl(model);
                    }),
                new PublicationType(TechnicalReportModel.NAME, technicalReportModel,
                    (model) => {
                        return new TechnicalReportUserControl(model);
                    }),
                new PublicationType(QualificationThesisModel.NAME, qualificationThesisModel,
                    (model) => {
                        return new QualificationThesisUserControl(model);
                    }),
            };

            return publicationTypes;
        }

        /// <summary>
        /// Načte testovací text zadané publikace.
        /// </summary>
        /// <param name="entry">publikace</param>
        private static string readText(Publication publication)
        {
            return File.ReadAllText(DIRECTORY + publication.Entry + EXTENSION);
        }

        /// <summary>
        /// Naplní databázi testovacími daty.
        /// </summary>
        private static void initializeData(AuthorModel authorModel, AttachmentModel attachmentModel,
            APublicationModel conferenceArticleModelArg,
            APublicationModel journalArticleModelArg,
            APublicationModel technicalReportModelArg,
            APublicationModel qualificationThesisModelArg)
        {
            ConferenceArticleModel conferenceArticleModel = conferenceArticleModelArg as ConferenceArticleModel;
            JournalArticleModel journalArticleModel = journalArticleModelArg as JournalArticleModel;
            TechnicalReportModel technicalReportModel = technicalReportModelArg as TechnicalReportModel;
            QualificationThesisModel qualificationThesisModel = qualificationThesisModelArg as QualificationThesisModel;

            Publication publication;
            List<Author> authors;
            ConferenceArticle conferenceArticle;
            JournalArticle journalArticle;
            TechnicalReport technicalReport;
            QualificationThesis qualificationThesis;

            // Logika není matematika
            publication = new Publication();
            publication.Entry = "logika-matematika";
            publication.Title = "Logika není matematika";
            publication.Year = 2005;
            publication.Text = readText(publication);
            publication.Type = ConferenceArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Ludmila", Surname = "Dostálová" },
            };
            conferenceArticle = new ConferenceArticle();
            conferenceArticle.BookTitle = "Univerzita: tradiční a netradiční ve výuce";
            conferenceArticle.Address = "Plzeň";
            conferenceArticle.Publisher = "Čeněk";
            conferenceArticle.FromPage = 162;
            conferenceArticle.ToPage = 170;
            conferenceArticle.ISBN = "80‑86898‑60‑1";
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);

            // Mobilní laserové pracoviště
            publication = new Publication();
            publication.Entry = "mobilni-pracoviste";
            publication.Title = "Mobilní laserové pracoviště";
            publication.Year = 2011;
            publication.Text = readText(publication);
            publication.Type = ConferenceArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Pavla", Surname = "Klufová" },
                new Author() { Name = "Stanislav", Surname = "Němeček" },
                new Author() { Name = "Tomáš", Surname = "Mužík" },
            };
            conferenceArticle = new ConferenceArticle();
            conferenceArticle.BookTitle = "Využití laserů v průmyslu";
            conferenceArticle.Address = "Brno";
            conferenceArticle.Publisher = "Tribun EU";
            conferenceArticle.FromPage = 83;
            conferenceArticle.ToPage = 86;
            conferenceArticle.ISBN = "978-80-554-0416-5";
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);

            // Vlivy pulzních modulací měničů na harmonické proudy
            publication = new Publication();
            publication.Entry = "vlivy-modulaci";
            publication.Title = "Vlivy pulzních modulací měničů na harmonické proudy";
            publication.Year = 2014;
            publication.Text = readText(publication);
            publication.Type = ConferenceArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Václav", Surname = "Kůs" },
                new Author() { Name = "Tereza", Surname = "Josefová" },
            };
            conferenceArticle = new ConferenceArticle();
            conferenceArticle.BookTitle = "Kopes 2014: kolokvium pedagogů elektrických strojů: sborník příspěvků z mezinárodní konference";
            conferenceArticle.Address = "Liberec";
            conferenceArticle.Publisher = "TU Liberec";
            conferenceArticle.FromPage = 62;
            conferenceArticle.ToPage = 66;
            conferenceArticle.ISBN = "978-80-7494-034-7";
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);

            // Deformation method for 3D identikit creation
            publication = new Publication();
            publication.Entry = "deformation-method";
            publication.Title = "Deformation method for 3D identikit creation";
            publication.Year = 2014;
            publication.Text = readText(publication);
            publication.Type = ConferenceArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Petr", Surname = "Martínek" },
                new Author() { Name = "Ivana", Surname = "Kolingerová" },
            };
            conferenceArticle = new ConferenceArticle();
            conferenceArticle.BookTitle = "GRAPP 2014: proceedings of the 9th international conference on computer graphics theory and applications";
            conferenceArticle.Address = "Setúbal";
            conferenceArticle.Publisher = "SciTePress";
            conferenceArticle.FromPage = 104;
            conferenceArticle.ToPage = 111;
            conferenceArticle.ISBN = "978-989-758-002-4";
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);

            // Šest let výuky oboru knihovnické a informační systémy a služby
            publication = new Publication();
            publication.Entry = "vyuka-oboru";
            publication.Title = "Šest let výuky oboru knihovnické a informační systémy a služby";
            publication.Year = 2010;
            publication.Text = readText(publication);
            publication.Type = ConferenceArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Daniela", Surname = "Novotná" },
            };
            conferenceArticle = new ConferenceArticle();
            conferenceArticle.BookTitle = "Knihovny současnosti 2010: sborník z 18. konference, konané ve dnech 14.‑16. září 2010 v Seči u Chrudimi";
            conferenceArticle.Address = "Ostrava";
            conferenceArticle.Publisher = "Sdružení knihoven ČR";
            conferenceArticle.FromPage = 168;
            conferenceArticle.ToPage = 173;
            conferenceArticle.ISBN = "978‑80‑86249‑59‑9";
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);

            // Feature space reduction and decorrelation in a large number of speech recognition experiments
            publication = new Publication();
            publication.Entry = "space-reduction";
            publication.Title = "Feature space reduction and decorrelation in a large number of speech recognition experiments";
            publication.Year = 2007;
            publication.Text = readText(publication);
            publication.Type = ConferenceArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Josef V.", Surname = "Psutka" },
                new Author() { Name = "Luboš", Surname = "Šmídl" },
                new Author() { Name = "Luděk", Surname = "Müller" },
                new Author() { Name = "Josef", Surname = "Psutka" },
            };
            conferenceArticle = new ConferenceArticle();
            conferenceArticle.BookTitle = "Signal and image processing: proceedings of the ninth IASTED internation conference: August 20‑22, 2007, Honolulu, Hawaii, USA";
            conferenceArticle.Address = "Anaheim";
            conferenceArticle.Publisher = "ACTA Press";
            conferenceArticle.FromPage = 158;
            conferenceArticle.ToPage = 161;
            conferenceArticle.ISBN = "978‑0‑88986‑675‑1";
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);

            // Gaussian mixtures proposal density in particle filter for track‑before‑detect
            publication = new Publication();
            publication.Entry = "gaussian-mixtures";
            publication.Title = "Gaussian mixtures proposal density in particle filter for track‑before‑detect";
            publication.Year = 2009;
            publication.Text = readText(publication);
            publication.Type = ConferenceArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Ondřej", Surname = "Straka" },
                new Author() { Name = "Miroslav", Surname = "Šimandl" },
                new Author() { Name = "Jindřich", Surname = "Duník" },
            };
            conferenceArticle = new ConferenceArticle();
            conferenceArticle.BookTitle = "Fusion 2009: the 12th international conference on information fusion";
            conferenceArticle.Address = "New York";
            conferenceArticle.Publisher = "IEEE";
            conferenceArticle.FromPage = 270;
            conferenceArticle.ToPage = 277;
            conferenceArticle.ISBN = "978‑0‑9824438‑0‑4";
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);

            // Utilization of activating teaching tools in education
            publication = new Publication();
            publication.Entry = "tools-utilization";
            publication.Title = "Utilization of activating teaching tools in education";
            publication.Year = 2013;
            publication.Text = readText(publication);
            publication.Type = ConferenceArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Lukáš", Surname = "Štich" },
                new Author() { Name = "Petr", Surname = "Simbartl" },
            };
            conferenceArticle = new ConferenceArticle();
            conferenceArticle.BookTitle = "Quality and efficiency in e-learning. Vol. 2: 9th international conference eLearning and Software for Education";
            conferenceArticle.Address = "Bucharest";
            conferenceArticle.Publisher = "Carol I' National Defence University Publishing House";
            conferenceArticle.FromPage = 173;
            conferenceArticle.ToPage = 176;
            conferenceArticle.ISSN = "2066-026X";
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);

            // Archeologický výzkum vesnic středověkého původu na Tachovsku zaniklých po roce 1945
            publication = new Publication();
            publication.Entry = "archeologicky-vyzkum";
            publication.Title = "Archeologický výzkum vesnic středověkého původu na Tachovsku zaniklých po roce 1945";
            publication.Year = 2008;
            publication.Text = readText(publication);
            publication.Type = ConferenceArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Pavel", Surname = "Vařeka" },
            };
            conferenceArticle = new ConferenceArticle();
            conferenceArticle.BookTitle = "Archaeologia historica";
            conferenceArticle.Address = "Bucharest";
            conferenceArticle.Publisher = "Muzejní a vlastivědná společnost";
            conferenceArticle.FromPage = 101;
            conferenceArticle.ToPage = 117;
            conferenceArticle.ISBN = "978‑80‑7275‑076‑4";
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);

            // Aktuální trendy v oblasti elektroniky
            publication = new Publication();
            publication.Entry = "aktualni-trendy";
            publication.Title = "Aktuální trendy v oblasti elektroniky";
            publication.Year = 2006;
            publication.Text = readText(publication);
            publication.Type = JournalArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Petr", Surname = "Beneš" },
            };
            journalArticle = new JournalArticle();
            journalArticle.JournalTitle = "Sdělovací technika: telekomunikace, elektronika, multimédia. Praha: Sdělovací technika";
            journalArticle.Number = "54(12)";
            journalArticle.FromPage = 3;
            journalArticle.ToPage = 6;
            journalArticle.ISSN = "0036‑9942";
            journalArticleModel.CreatePublication(publication, authors, journalArticle);

            // Máje z okolí uhlířsko‑janovického
            publication = new Publication();
            publication.Entry = "maje-okoli";
            publication.Title = "Máje z okolí uhlířsko‑janovického";
            publication.Year = 1895;
            publication.Text = readText(publication);
            publication.Type = JournalArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Václav", Surname = "Ešnei" },
            };
            journalArticle = new JournalArticle();
            journalArticle.JournalTitle = "Český lid. Praha: V. Šimáček";
            journalArticle.Number = "4(1)";
            journalArticle.FromPage = 4;
            journalArticle.ToPage = 8;
            journalArticle.ISSN = "0000‑0000";
            journalArticleModel.CreatePublication(publication, authors, journalArticle);

            // Technologie výroby biopaliv druhé generace
            publication = new Publication();
            publication.Entry = "technologie-vyroby";
            publication.Title = "Technologie výroby biopaliv druhé generace";
            publication.Year = 2010;
            publication.Text = readText(publication);
            publication.Type = JournalArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Jan", Surname = "Hromádko" },
                new Author() { Name = "Jiří", Surname = "Hromádko" },
                new Author() { Name = "Petr", Surname = "Miler" },
                new Author() { Name = "Vladimír", Surname = "Hönig" },
                new Author() { Name = "Martin", Surname = "Cindr" },
            };
            journalArticle = new JournalArticle();
            journalArticle.JournalTitle = "Chemické listy";
            journalArticle.Number = "104(8)";
            journalArticle.FromPage = 784;
            journalArticle.ToPage = 790;
            journalArticle.ISSN = "0009‑2770";
            journalArticleModel.CreatePublication(publication, authors, journalArticle);

            // Sezónní dynamika zooplanktonu rybníka Vydymáček u Plzně
            publication = new Publication();
            publication.Entry = "sezonni-dynamika";
            publication.Title = "Sezónní dynamika zooplanktonu rybníka Vydymáček u Plzně";
            publication.Year = 2014;
            publication.Text = readText(publication);
            publication.Type = JournalArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Veronika", Surname = "Kreidlová" },
                new Author() { Name = "Milan", Surname = "Šorf" },
                new Author() { Name = "Jiří", Surname = "Kout" },
            };
            journalArticle = new JournalArticle();
            journalArticle.JournalTitle = "Erica";
            journalArticle.Number = "21(1)";
            journalArticle.FromPage = 141;
            journalArticle.ToPage = 160;
            journalArticle.ISSN = "1210-065X";
            journalArticleModel.CreatePublication(publication, authors, journalArticle);

            // Josef Kajetán Tyl a západní Čechy
            publication = new Publication();
            publication.Entry = "tyl-cechy";
            publication.Title = "Josef Kajetán Tyl a západní Čechy";
            publication.Year = 2006;
            publication.Text = readText(publication);
            publication.Type = JournalArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Jan", Surname = "Kumpera" },
            };
            journalArticle = new JournalArticle();
            journalArticle.JournalTitle = "Plzeňský deník. 10. 7. 2006";
            journalArticle.Number = "15(158)";
            journalArticle.FromPage = 3;
            journalArticle.ToPage = 3;
            journalArticle.ISSN = "1210‑5139";
            journalArticleModel.CreatePublication(publication, authors, journalArticle);

            // Genderové aspekty stárnutí: rodina a péče o seniory
            publication = new Publication();
            publication.Entry = "genderove-aspekty";
            publication.Title = "Genderové aspekty stárnutí: rodina a péče o seniory";
            publication.Year = 2006;
            publication.Text = readText(publication);
            publication.Type = JournalArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Kamila", Surname = "Svobodová" },
            };
            journalArticle = new JournalArticle();
            journalArticle.JournalTitle = "Demografie";
            journalArticle.Number = "48(4)";
            journalArticle.FromPage = 256;
            journalArticle.ToPage = 261;
            journalArticle.ISSN = "0011‑8265";
            journalArticleModel.CreatePublication(publication, authors, journalArticle);

            // Ion flux characteristics in pulsed dual magnetron discharges used for deposition of photoactive TiO2 films
            publication = new Publication();
            publication.Entry = "flux-characteristics";
            publication.Title = "Ion flux characteristics in pulsed dual magnetron discharges used for deposition of photoactive TiO2 films";
            publication.Year = 2011;
            publication.Text = readText(publication);
            publication.Type = JournalArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Jan et al", Surname = "Šícha" },
            };
            journalArticle = new JournalArticle();
            journalArticle.JournalTitle = "Plasma Processes and Polymers";
            journalArticle.Number = "8(3)";
            journalArticle.FromPage = 191;
            journalArticle.ToPage = 199;
            journalArticle.ISSN = "1612‑8850";
            journalArticleModel.CreatePublication(publication, authors, journalArticle);

            // Archeologický výzkum vesnic středověkého původu na Tachovsku zaniklých po roce 1945
            publication = new Publication();
            publication.Entry = "vyzkum-casopis";
            publication.Title = "Archeologický výzkum vesnic středověkého původu na Tachovsku zaniklých po roce 1945";
            publication.Year = 2008;
            publication.Text = readText(publication);
            publication.Type = JournalArticleModel.NAME;
            authors = new List<Author>()
            {
                authorModel.GetAuthorById(19), // Pavel Vařeka
            };
            journalArticle = new JournalArticle();
            journalArticle.JournalTitle = "Archaeologia historica";
            journalArticle.Number = "33";
            journalArticle.FromPage = 101;
            journalArticle.ToPage = 117;
            journalArticle.ISSN = "0231‑5823";
            journalArticleModel.CreatePublication(publication, authors, journalArticle);

            // Příspěvek ke studiu žijících vsí středověkého původu. Pozůstatky zástavby z pozdního středověku na parcele č.p. 121 v Mikulčicích
            publication = new Publication();
            publication.Entry = "prispevek-studiu";
            publication.Title = "Příspěvek ke studiu žijících vsí středověkého původu. Pozůstatky zástavby z pozdního středověku na parcele č.p. 121 v Mikulčicích";
            publication.Year = 2010;
            publication.Text = readText(publication);
            publication.Type = JournalArticleModel.NAME;
            authors = new List<Author>()
            {
                authorModel.GetAuthorById(19), // Pavel Vařeka
            };
            journalArticle = new JournalArticle();
            journalArticle.JournalTitle = "Přehled výzkumů";
            journalArticle.Number = "51(1‑2)";
            journalArticle.FromPage = 249;
            journalArticle.ToPage = 265;
            journalArticle.ISSN = "1211‑7250";
            journalArticleModel.CreatePublication(publication, authors, journalArticle);

            // Directed Design of Thermal Insulation System of Contemporary Buildings Based on Hygrothermal Analysis - Progress Report
            publication = new Publication();
            publication.Entry = "directed-design";
            publication.Title = "Directed Design of Thermal Insulation System of Contemporary Buildings Based on Hygrothermal Analysis - Progress Report";
            publication.Year = 2010;
            publication.Text = readText(publication);
            publication.Type = TechnicalReportModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "R.", Surname ="Černý" },
            };
            technicalReport = new TechnicalReport();
            technicalReport.Address = "Praha";
            technicalReport.Institution = "ČVUT v Praze, Fakulta stavební, Katedra materiálového inženýrství a chemie";
            technicalReport.Number = "Research Report ME10112/1";
            technicalReportModel.CreatePublication(publication, authors, technicalReport);

            // Měření charakteristik vzduchu po průchodu vodní clonou
            publication = new Publication();
            publication.Entry = "mereni-charakteristik";
            publication.Title = "Měření charakteristik vzduchu po průchodu vodní clonou";
            publication.Year = 2010;
            publication.Text = readText(publication);
            publication.Type = TechnicalReportModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "L.", Surname ="Dvořák" },
                new Author() { Name = "J.", Surname ="Čížek" },
                new Author() { Name = "L.", Surname ="Nováková" },
                new Author() { Name = "J.", Surname ="Kolínský" },
                new Author() { Name = "P.", Surname ="Vitkovič" },
            };
            technicalReport = new TechnicalReport();
            technicalReport.Address = "Praha";
            technicalReport.Institution = "České vysoké učení technické v Praze";
            technicalReport.Number = "Výzkumná zpráva č. Z-271/11";
            technicalReportModel.CreatePublication(publication, authors, technicalReport);

            // Analýza prostorových a formálních vlastností středověkých obléhacích táborů
            publication = new Publication();
            publication.Entry = "analyza-vlastnosti";
            publication.Title = "Analýza prostorových a formálních vlastností středověkých obléhacích táborů";
            publication.Year = 2010;
            publication.Text = readText(publication);
            publication.Type = QualificationThesisModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Petr", Surname = "Koscelník" },
            };
            qualificationThesis = new QualificationThesis();
            qualificationThesis.Address = "Plzeň";
            qualificationThesis.School = "Západočeská univerzita v Plzni. Fakulta filozofická";
            qualificationThesis.ThesisType = QualificationThesisModel.TYPE_MASTER_THESIS;
            qualificationThesisModel.CreatePublication(publication, authors[0], qualificationThesis);

            // Teoretické a experimentální studium problematiky HSC obrábění ocelí vysoké pevnosti a tvrdosti
            publication = new Publication();
            publication.Entry = "studium-problematiky";
            publication.Title = "Teoretické a experimentální studium problematiky HSC obrábění ocelí vysoké pevnosti a tvrdosti";
            publication.Year = 2004;
            publication.Text = readText(publication);
            publication.Type = QualificationThesisModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Jan", Surname = "Řehoř" },
            };
            qualificationThesis = new QualificationThesis();
            qualificationThesis.Address = "Plzeň";
            qualificationThesis.School = "Západočeská univerzita. Fakulta strojní. Katedra technologie obrábění";
            qualificationThesis.ThesisType = QualificationThesisModel.TYPE_PHD_THESIS;
            qualificationThesisModel.CreatePublication(publication, authors[0], qualificationThesis);
        }
    }
}

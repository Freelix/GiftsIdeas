using System.Web;
using System.Web.Optimization;

namespace GiftsManager
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/Scripts/Common/jquery-1.10.2.min.js",
                        "~/Assets/Scripts/Common/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Assets/Scripts/Common/Site.js",
                        "~/Assets/Scripts/Common/sweetalert2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/Scripts/Common/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour le développement et l'apprentissage. Puis, une fois
            // prêt pour la production, utilisez l'outil de génération (bluid) sur http://modernizr.com pour choisir uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Assets/Scripts/Common/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Assets/Scripts/Common/bootstrap.min.js",
                      "~/Assets/Scripts/Common/respond.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Assets/Styles/bootstrap.min.css",
                      "~/Assets/Styles/Site.css",
                      "~/Assets/Styles/jquery-ui.min.css",
                      "~/Assets/Styles/jquery-ui.theme.min.css",
                      "~/Assets/Styles/jquery.akordeon.css",
                      "~/Assets/Styles/sweetalert2.css"));
        }
    }
}

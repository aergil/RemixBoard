using RemixBoard.Core;

namespace RemixBoard.Wpf
{
   public class JobHtml
    {
       private Job job;

       public JobHtml(Job job) {
           this.job = job;
       }

       public string Description() {
           var experience = !string.IsNullOrEmpty(job.Expérience) ? string.Format(htmlParagraphe, "Expérience", job.Expérience) : string.Empty;
           var etudes = !string.IsNullOrEmpty(job.Etudes) ? string.Format(htmlParagraphe, "Etudes", job.Etudes) : string.Empty;
           var description = !string.IsNullOrEmpty(job.Description) ? string.Format(htmlParagraphe, "Description", job.Description) + "<br/>" : string.Empty;

           return string.Format(htmlDeclaration, job.Titre, experience, etudes, description, style);
       }

       private const string htmlDeclaration = @"
<head>
    <meta http-equiv='Content-Type' content='text/html;charset=UTF-8'>
    {4}
</head>
<body>
    <div class=""DescriptionTitre1"">{0}</div>
    {1}
    {2}      
    {3}                              
</body>";
       private const string htmlParagraphe = @"<p><span class=""DescriptionTitre2"">{0} : </span>{1}</p>";
       private const string style = @"
<style type=""text/css""> 
    body { font-family : ""Segoe UI""; font-size : 11; }
    .DescriptionTitre1 { Color : MediumBlue; font-size : 16;} 
    .DescriptionTitre2 { Color :  MediumBlue; font-size : 14;}
</style>";
    }
}

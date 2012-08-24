Remix Board :

- Une application desktop .NET en C#.
- Exploite ces 2 flux JSON d'offres d'emplois pour me les pr�senter : http://www.express-board.fr/api/jobs et https://remixjobs.com/api/jobs.json
- Acc�s aux offres sur le site de l'annonce � partir d'un lien cliquable dans l'application.
- Filtres sur ces annonces : mots clefs, ville, type de contrat
- Mode hors ligne avec les derni�res offres connues
- Annonces en favoris


Architecture
------------

Application :
- RemixBoard.Wpf : Couche de pr�sentation
- RemixBoard.Core : Corps de l'application, avec r�cup�ration des flux JSON
- RemixBoard.Datas : Implemenation de la couche de persistance : NHibernate, requetage au travers de NHibernate.Linq
Projets Annexes :
- RemixBoard.Test : Tests du Core et de Datas
- Setup : Installeur de l'application
	
	
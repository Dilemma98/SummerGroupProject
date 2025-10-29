# SummerGroupProject

Detta är ett sommarprojekt mellan WU-24-studenterna [Tuva Gyllensten](https://github.com/gytu24nn), [Tintin Larsson](https://github.com/Controlfox) och [Emma Högdal](https://github.com/Dilemma98) – skapat för att hålla kunskaperna vid liv under sommaren inför år 2. 🌞

Projektet består av en liten blogg där vi kan uppdatera varandra om våra sommaraktiviteter.

---

## 🧭 Kom igång

### 1. Klona ner projektet
Börja med att klona ner projektet i valfri mapp:

```bash
git clone https://github.com/Dilemma98/SummerGroupProject.git
2. Backend
Gå till backend-mappen:

bash
Kopiera kod
cd backend
Installera beroenden och starta backend:

bash
Kopiera kod
dotnet restore
dotnet run
Backend körs då på http://localhost:5196.

3. Frontend
Gå till frontend-mappen:

bash
Kopiera kod
cd frontend
Installera beroenden:

bash
Kopiera kod
npm install
Starta frontend:

bash
Kopiera kod
npm run dev
Frontend körs på http://localhost:5173.

🔑 Google API Setup
1. OAuth 2.0 Client ID (för inloggning)
Gå till Google Cloud Console.

Skapa ett nytt projekt eller välj ett befintligt.

Navigera till API & Services → Credentials.

Klicka på Create Credentials → OAuth 2.0 Client ID.

Välj applikationstyp Web application.

Ange redirect URI:

arduino
Kopiera kod
http://localhost:5173
Kopiera Client ID och skapa en .env-fil i frontend-mappen:

bash
Kopiera kod
VITE_GOOGLE_CLIENT_ID="din-client-id"
2. Google Sheets Setup (för att lagra poster)
Skapa ett Google Sheet med kolumner:

nginx
Kopiera kod
Title | Content | ImageUrl | Author | AuthorImgUrl | CreatedAt
Skapa en Google Service Account i Google Cloud:

Gå till IAM & Admin → Service Accounts → Create Service Account

Ge den ett namn, t.ex. sheetswriter.

Ge rollen Editor (för att kunna skriva till Sheet).

Ladda ner JSON-nyckeln (service-account.json).

Dela ditt Google Sheet med service account email (t.ex. sheetswriter@summergroupproject.iam.gserviceaccount.com) med Editor-rättigheter.

Lägg JSON-filen i backend-projektets rotmapp och se till att filen heter exakt:

bash
Kopiera kod
backend/service-account.json
Lägg Sheet ID i appsettings.json:

json
Kopiera kod
{
  "GOOGLE_SHEET_ID": "ditt-sheet-id"
}
Du hittar Sheet ID i URL:en till ditt Google Sheet, t.ex. https://docs.google.com/spreadsheets/d/<sheet-id>/edit.

⚙️ API Endpoints
GET /api/posts – Hämta alla poster

POST /api/posts – Skapa ny post (kräver inloggning)

PATCH /api/posts/{rowNumber} – Uppdatera post

DELETE /api/posts/{rowNumber} – Ta bort post

📝 Testa projektet
Öppna frontend i webbläsaren: http://localhost:5173.

Logga in med Google.

Lägg till, redigera och ta bort poster.

Klicka på bilder för fullscreen-visning.

⚠️ Noteringar
Endast inloggade användare kan skapa poster.

Poster lagras i Google Sheets via service account.

Bilder lagras lokalt i wwwroot/uploads.
# SummerGroupProject

Detta är ett sommarprojekt mellan WU-24-studenterna [Tuva Gyllensten](https://github.com/gytu24nn), [Tintin Larsson](https://github.com/Controlfox) och [Emma Högdal](https://github.com/Dilemma98) – skapat för att hålla kunskaperna vid liv under sommaren inför år 2. 🌞

Projektet består av en liten blogg där vi kan uppdatera varandra om våra sommaraktiviteter.

## 🧭 Kom igång

### 1. Klona ner projektet
Börja med att klona ner projektet i valfri mapp:
```bash
git clone https://github.com/Dilemma98/SummerGroupProject.git

```
### 2. Backend
```bash
cd backend
dotnet restore
dotnet run
```

### 3. Frontend
```bash
cd frontend
npm install
npm run dev
```

#### Google API - OAuth client ID
För att möjliggöra inloggning med Google och åtkomst till kalendern behöver du skapa ett eget OAuth 2.0 Client ID via Google Cloud Console.

1. Gå till Google Cloud Console.

2. Skapa ett nytt projekt eller välj ett befintligt projekt.

3. Gå till API & Services > Credentials.

4. Klicka på Create Credentials och välj OAuth 2.0 Client IDs.

5. Välj en lämplig applikationstyp (vanligtvis "Web application").

6. När du skapar ditt Client ID, kommer du behöva ange en redirect URI. Där klistrar du in http://localhost:5173

7. När du har skapat din OAuth 2.0 Client ID, kopiera client_id och skapa en .env- fil i roten av frontend-mappen där du lägger in detta.

```bash
VITE_GOOGLE_CLIENT_ID="din-client-id"
```
Och byt ut "din-client-id" mot den du fick från Google Cloud Console
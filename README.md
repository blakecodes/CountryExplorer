# CountryExplorer

## Running with Docker Compose

The simplest way to spin up both the API and the UI together is with Docker Compose. From the project root, run:

```bash
docker-compose up --build
```

This will:
1. Build and start the .NET API container (exposed on port 5117).
2. Build the Angular UI, serve it with Nginx, and expose it on port 4200.

Once the containers are running, open your browser at [http://localhost:4200](http://localhost:4200). The UI will communicate with the API container automatically.

---

## Running the UI (Angular)

1. Open a terminal and navigate to the `CountryExplorer.UI` directory:
   
   ```bash
   cd CountryExplorer.UI
   ```
2. Install dependencies:
   
   ```bash
   npm install
   ```
3. Start the development server:
   
   ```bash
   npm start
   ```
4. Open your browser and go to [http://localhost:4200](http://localhost:4200)

---

## Running the API (.NET)

1. Open a terminal and navigate to the `CountryExplorer.API` directory:
   
   ```bash
   cd CountryExplorer.API
   ```
2. Run the API:
   
   ```bash
   dotnet run
   ```
3. The API will be available at [https://localhost:7173](https://localhost:7173) (or [http://localhost:5117](http://localhost:5117)).
4. Swagger UI for API testing is available at `/swagger` (e.g., [https://localhost:7173/swagger](https://localhost:7173/swagger)).

---

For more details, see the `README.md` files in each project folder (if available).

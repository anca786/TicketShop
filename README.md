# 🎫 TicketShop - Platformă de E-Ticketing

**TicketShop** este o aplicație web completă pentru vânzarea și gestionarea biletelor la evenimente (concerte, teatru, conferințe), dezvoltată folosind **ASP.NET Core MVC**. Aplicația simulează un flux complet de e-commerce, de la navigare și căutare, până la achiziție și generarea biletelor electronice.

---

## 🚀 Tehnologii Utilizate

* **Framework:** .NET 6 / .NET 7 (ASP.NET Core MVC)
* **Limbaj:** C#
* **Baza de date:** Microsoft SQL Server & Entity Framework Core (Code-First)
* **Authentication:** ASP.NET Core Identity
* **Frontend:** HTML5, CSS3, Bootstrap 5, Razor Views
* **Tools:** Visual Studio 2022

---

## ✨ Funcționalități Principale

### 👤 Pentru Utilizatori (Clienți)
* **Navigare & Căutare:** Vizualizare evenimente, filtrare după categorii, bară de căutare inteligentă.
* **Coș de Cumpărături:** Adăugare bilete, verificare disponibilitate stoc în timp real, finalizare comandă.
* **Profil Utilizator (Dashboard):**
    * Statistici personale (total cheltuit, număr bilete).
    * **Bilete Digitale:** Design tip "Boarding Pass" cu **Cod QR generat automat** pentru fiecare bilet.
    * Istoric comenzi și recenzii.
* **Setări Cont:** Posibilitatea de a modifica datele personale și parola.
* **Wishlist:** Salvarea evenimentelor favorite.
* **Sistem de Recenzii:** Posibilitatea de a lăsa review-uri la evenimentele trecute.

### 🛡️ Pentru Administratori
* **Panou de Administrare:** Acces securizat pe bază de roluri (`Admin`).
* **Gestionează Evenimente:** Adăugare, editare, ștergere evenimente (CRUD), upload imagini.
* **Gestionează Categorii:** Organizarea evenimentelor.
* **User Management:** Vizualizarea listei de utilizatori și gestionarea accesului.

---

## 📸 Capturi de Ecran (Opțional)
*(Aici poți pune link-uri către imagini dacă urci proiectul pe GitHub, de exemplu: Pagina de Home, Profilul cu QR Code, Coșul)*

---

## ⚙️ Cum să rulezi proiectul local

1.  **Clonează repository-ul:**
    ```bash
    git clone [https://github.com/userul-tau/TicketShop.git](https://github.com/userul-tau/TicketShop.git)
    ```
2.  **Deschide proiectul** în Visual Studio (`TicketShop.sln`).
3.  **Configurează Baza de Date:**
    * Deschide `appsettings.json` și verifică `ConnectionStrings`. Asigură-te că serverul SQL (LocalDB sau SQL Express) este corect.
4.  **Aplică Migrările:**
    * Deschide **Package Manager Console** (Tools -> NuGet Package Manager).
    * Rulează comanda:
        ```powershell
        Update-Database
        ```
    * *Această comandă va crea baza de date și va popula tabelele (Seeding) cu datele inițiale.*
5.  **Rulează aplicația:**
    * Apasă `F5` sau butonul de "Run" din Visual Studio.

---

## 🔐 Credențiale (Pentru Testare)

Dacă s-a rulat `DbSeeder`, poți folosi următoarele conturi predefinite:

* **Admin:** `admin@shop.ro` / Parola: `DemoUser123!`
* **Client:** `client@shop.ro` / Parola: `DemoUser123!`
* **Colaborator:** `colaborator@shop.ro` / Parola: `DemoUser123!`

---

## 📝 Licență

Acest proiect este realizat în scop educațional.

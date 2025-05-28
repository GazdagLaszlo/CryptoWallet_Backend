# CryptoWallet Backend 

A projekt célja egy kriptovaluta adás-vétel szimulátor backend funkcionálisának megvalósítása. A rendszer lehetőséget biztosít a felhasználók számára, hogy egy virtuális pénztárcával kriptovalutákat vásároljanak, tartsanak meg, illetve adjanak el a piaci árfolyam változásának figyelembevételével.

---

## Technológiai környezet

- Backend: **C# .NET 9**
- Adatbázis: **Microsoft SQL Server (MSSQL)**  
- ORM: **Entity Framework Core**
- API dokumentáció: **Swagger**

---

## Főbb funkciók
- Felhasználói regisztráció, adatlekérdezés, módosítás és törlés
- Felhasználói pénztárca kezelése (egyenleg és kriptovaluták)
- 15 előre definiált kriptovaluta kezelése, árfolyamok dinamikus frissítése api segítségével
- Kriptovaluták vásárlása és eladása a virtuális pénztárca egyenlegének megfelelően
- Árfolyamváltozások automatikus és manuális kezelése, árfolyamváltozási naplózás
- Profit és veszteség számítása portfólió alapján
- Tranzakciók naplózása és lekérdezése

---

## Telepítés és futtatás

1. Klónozd a repository-t
https://github.com/GazdagLaszlo/CryptoWallet_Backend.git
2. Nyisd meg a projektet Visual Studio-ban vagy az általad preferált IDE-ben.
3. Állítsd be az adatbázis kapcsolat stringet
4. Futtasd az adatbázis migrációkat
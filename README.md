1. **A POST endpoint to add a wallet** with the following business rules:
   - Prevent duplicate wallet additions.
   - A single user should NOT have more than 5 wallets.
   - Only the first 6 digits of the card number should be stored.

2. **A DELETE endpoint to remove a wallet**.

3. **A GET endpoint to retrieve a single wallet using an ID**.

4. **A GET endpoint to list all wallets**.

5. **Add Unit Tests** using the xUnit testing framework.

### Wallet Model:

- ID
- Name
- Type (Mobile Money or Card only)
- Account Number (Mobile Money number or Card number)
- Account Scheme (Visa, Mastercard, MTN, Vodafone, AirtelTigo)
- Created At
- Owner - Phone number of the person that owns the wallet

### Framework:

.NET 6

### Storage:

Choose from In-memory, RDMS (Relational Database Management System), or NoSQL storage.

### Demonstration:

Demonstrate the finished work using POSTman or Swagger.

### GitHub Repository:

Please forward a link to your work on GitHub or any Version Control System (VCS) provider.

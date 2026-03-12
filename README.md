# Time Spent
Estimated time: 10-12 hours  
Actual time: 12 hours

The majority of the time I spent on this development practice involved connecting and creating a database using EF core. This was something I was unfamiliar with, so I needed extra time for research and also ran into many errors when testing this functionality.

# Approach & Assumptions
I first approached creating this project by splitting the 3 main functionalities into different classes. Once I had an idea of what I wanted each of the classes to do, I coded each class separately. I then developed tests for each of the classes, which was much easier because I had broken up the 3 functionalities into different classes and parts. Finally, I then finished the Worker class by combining the 3 steps into one file, where the result of one class was used as an input for another.  

One of the assumptions I made was that the JSON calls would also include a Status and TransactionChanges field, and also made the assumption that these 5 fields I made within the TransactionChange class were enough to detail a change made to a transaction. I go more into detail about this functionality under the JSON Formatting section below.

# JSON Formatting
I edited the JSON calls by adding a Status (represented by a boolean, i.e. whether the transaction has been revoked) and list of Transaction Changes to represent the changes made to each transaction. Each transaction change has 5 fields: the transaction ID of the transaction being updated, the specific field that was updated, the old value, the new value, and the time that the update was made. This allowed me to fulfill the acceptance criteria of marking transactions as revoked and also recording edits made to each transaction.

# Build Steps
The TransactionIngest project can be run by configuring the startup projects and clicking `TransactionInjest` under "Single startup project:" within the Configure Startup Projects tab. Once the program is run, it uses the JSON code in `Data\MockTransactions.json` to create a `TransactionDb.db` file, which can be found within the `"TransactionIngest\bin\Debug\net10.0\Data"` folder. Here, the database should reflect the changes of adding new or updated transactions to the database. The data within the mock JSON file can be edited to test other functionalities (such as handling duplicates, revoking transactions, etc.)  

Each step within the Worker class is tested in `TransactionInjest.Tests`, which can be run by configuring the startup projects and clicking `TransactionInjest.Tests` under "Single startup project:" within the Configure Startup Projects tab. Here, each processor is tested (one for adding new transactions, one for updating new transactions, and one for revoking transactions) with different edge cases, such as testing idempotency, adding new transactions, updating previous transactions, revoking transactions within the last 24 hours, etc. and also the case where nothing is updated or revoked.  

Unit tests were also created to ensure transactions are correctly updated and that the list of Transaction Changes reflect these updates.

# Problems
One of the problems I ran into was connecting and creating the database. To confirm that the errors weren't being caused by my 3 processors, I made sure to first make tests to confirm that these processors were working as intended. Once I was able to successfully create these tests and verified that their functionality was what I expected, I then was able to eliminate any outside circumstances that could have caused the error. This allowed me to narrow down the possibilities, making it easier for me to find where the problem was.  

One other problem I ran into was expressing the different types of transaction changes. For example, changing the card number, location code, or product name involved updating strings, while changing the amount involved changing a decimal, and changing the transaction time involved changing a DateTime. This made it much harder for me to make the TransactionChange class, as this involved me having to cover multiple cases of different changes. I used strings to represent changes made to all of the fields, as this was a data type that could function the best in expressing all 3 data types. I assume that there is probably a much better way to express this data, however I was unable to explore any different methods due to the amount of time I had to complete this project.

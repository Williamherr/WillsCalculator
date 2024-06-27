# WillsCalculator

This project is a full stack calculator with angular as the front (willscalculator.client) and .Net core as the back (WillsCalculator.Server). 

The project has 1 parent (app) and 2 child components (calculator and history) in this application. There are 4 fields: username, first num, last num, and operations. Username must be field out first or else first and second number will be disabled. The user must enter all field and pass input validation then press the "Calculate Math" button to perform the operation. The results will show on the bottom of the form. The clear number button will revert the numbers in the form. The calculator.services.ts was used to handle any fetch and post to the api endpoints. 

History board will show a list of operations the user has done. It can be toggled by pressing on caret icon.   

Tailwind css and Daisy UI was used for the styling.

![image](https://github.com/Williamherr/WillsCalculator/assets/61044194/50075d2e-644d-420f-9fcb-6b88942f1381)

For the backend, I created 3 different folder for Controllers, Interfaces, and Services. Pretty much I connected services to the controller through the interfaces. 

The NumberStore was used to store the numbers/positions and the history board. A dictonary was used to keep track of the key, user, and their numbers/history. 
The CalculatorService was used to calculate any mathatical operations. 

# Techstack:

- Front End: Angular, Daisy UI, Tailwindcss, 
- Back End: .Net Core 8
- Testing: MSTest, jasmine

# TODO / How to improve
- Front End: Add login and allow history board to update after calculating math.
- Back End: Add authorization that way a user will not have to enter in a random username.
- Add more unit testing

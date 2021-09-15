# sightly-qa-exercise

This project navigates to an online report generator, grabs a report, and validates it against hard coded data.

## To Run:
Open the project in visual studio and use their Test Explorer window.
When the project is built, tests will display in the Test Explorer (XUnit is used for the tests).

### Details:
Two tests exist.  
One test performs the automated steps with a successful expected outcome.  This can be seen on the test output as so:
![alt text](https://github.com/ywapom/sightly-qa-exercise/blob/main/success_output.png)

The second test is a forced failure.  The test data was altered to mismatch the newly downloaded test.
The purpose of this was to display how failures are reported.  From the image you can see that each mismatch is reported:
![alt text](https://github.com/ywapom/sightly-qa-exercise/blob/main/failure_output.png)


### Important Note:
The directions for the exercise instructed testing against the first report in the list.
This resulted in an "Network Error" that occurred using various internet connections.  Therefore, the second report was used.
![alt text](https://github.com/ywapom/sightly-qa-exercise/blob/main/issue.png)

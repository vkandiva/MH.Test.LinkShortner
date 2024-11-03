# Link Shortener Application

A simple tool that will generate and redirect shortened urls to the original URLs

## Installation

Open the solution in Visual studio
Build the solution
Run the WebAPI project first
Then Run the UI project.

## Approach to designing and implementing a URL shortener service

The URL shortening basically uses random generator to generate a random string of 6 characters to generate and shortened URL from any given URL. This should give us 366^6 combinations and should generate unique URLs for each string. For more combinations, we could increase to more than 6 characters.

The URLs are stored in an in memory database which stores all the generated data till the WEB APIs are running. This could be further extended to a database of our choice.


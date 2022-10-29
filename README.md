# Pokédex

*By DutchSparklez, made for the "We are in IT together"-conference.*

This Pokédex is the subject of my presentation during the conference. The goal of the presentation is to inspire my peers by introducing them to a 'complete' software development project. During the presentation, I go into details on how a project like this is set up and made. This file is an addition to the presentation, containing an overview of the project and a short manual on how to use the code. I hope you're as exited as I am!

A detailed step by step tutorial (will be (partially)) available in text form (.md files) in the tutorial folder at the 18th of November. The slides (.pdf file) will be uploaded into the same folder. The code is also completely documented within the project using comments and relevant docstrings.

## Overview

### Web Crawler

*Made in Python with BeautifulSoup using Visual Studio Code.*

The first component of the project to be implemented was the webcrawler. This component yields the fastest output and therefore the right palce to start. The goal is simple: get the data on all pokemon from the first generation in a json file. The data is the Pokédex from https://pokemon.com/pokedex/.

The core of the crawler is the `get-data()` method within the `Pokemon` class. This function, as shown below, extracts data from the downloaded HTML file.  A single result needs cleaning, multiple results do not. So the first step is to checks whether we're expecting a collection of results. Using the DOM selector provided in the selector parameter, I get what I want out of the html.

```python
# Get specific data from the provided HTML DOM.
def get_data(self, html, selector, isDataCollection=False):
    # Check whether we're dealing with a collection of data
    if (isDataCollection): 
        # Return the collection requested
        return list(map(lambda item: item.text, html.select(selector)))

    # Return the element requested
    # We split the string in case there is a # in there, since this is the Pokedex number
    return html.select(selector)[0].text.replace("\n", "").lstrip().rstrip().split('#')
```

When a single item is expected, I perform some actions to santize the output. I remove all line-breaks; remove all the leading (`lstrip()`) and trailing (`rstrip()`) space. The `split('#')` is used to break a string up into two parts if a # is present. This is the case for the Pokémon's name and number.

For the presentation, only the first generation of Pokémon will be used. But if I (or anyone else) ever wants to expend this, the only thing they'd have to do is change the range of the loop at the start of the crawler to whatever they'd need:

```python
# Loop over the pokemon, we only want gen 1
for i in range(151):
    # Log to the console what's happening
    print("Collecting data on Pokémon #" + str(i + 1))
```

### Android App

*Made in Xamarin.Android (C#) using Visual Studio 2022 Community.*

Not yet finished...

### Web API

*Made in Node.js (JavaScript) with Express using Visual Studio Code.*

Not yet finished...

## Using the Pokédex

To be done...

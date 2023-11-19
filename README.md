# Pokédex

*By Jimmaphy, made for the "We are in IT together"-conference.*

This Pokédex is the subject of my presentation during the conference. The goal of the presentation is to inspire my peers by introducing them to a 'complete' software development project. During the presentation, I go into details on how a project like this is set up and made. This file is an addition to the presentation, containing an overview of the project and a short manual on how to use the code. I hope you're as exited as I am! The code is also completely documented within the project using comments and relevant docstrings.

## Overview

### Web Crawler

*Made in Python with BeautifulSoup using Visual Studio Code.*

The first component of the project to be implemented was the webcrawler. This component yields the fastest output and therefore the right palce to start. The goal is simple: get the data on all pokemon from the first generation in a json file. The data is the Pokédex from <https://pokemon.com/pokedex/>.

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

The development of the application started with loading the JSON. Android has two different folders: `assets` & `resources`. The 'resources' folder is used for images, audio, styling, and more of the likes; the 'assets' folder is used for everything else. Loading the JSON is pretty easy actually:

```csharp
PokedexService.LoadPokemonData(Assets.Open("pokemon.json"));
```

Now that there is data to play with, I can start using it. Creating a Pokémon class and a Pokédex service is the start of all of this. When implemented correctly; the code to actually load a random Pokémon for testing purposes becomes fairly easy:

```csharp
public static Pokemon GetRandomPokemon() => GetPokemon(new Random().Next(1, 152));
```

The next step is selecting a Pokémon from a list. Making the list involves Activities (a single screen in Android) and Intent (data for a transition to a different screen). There are two ways (more, but generally two) to start an activity: normally and with the intention to get a result from the activity. So I made an activity that has a list of Pokémon, and whenever a Pokémon is clicked; the following code is executed:

```csharp
pokemonList.ItemClick += (s, eventTrigger) => {
    (string number, string name) = pokemon.ElementAt(eventTrigger.Position);

    Intent intent = new Intent();
    intent.PutExtra("number", number);

    SetResult(Result.Ok, intent);
    Finish();
};
```

The camera is not a function I'm actively going to share. The functionality is bodged and not up to standards. Everything build around the camera is there just to make it work, not to make it right. The general functionality is comparable to the list of Pokémon. You start a new activity; which is the standard camera app on the phone. It takes a picture and sends it to Azure using HTTP POST.

### Image Recognition

*Made using Azure Custom Vision through HTTP REST.*

The simplest and most tedious task of the entire project. Collecting images on the first 151 Pokémon and uploading them to Azure is quite a repetitive task. The connection to the Azure service is done through HTTP. The app makes a request using HTTP POST. It get's predictions back, containing the match percentage and tag information. All the images are tagged with the Pokédex number of the Pokédex.

## Why this app is...

### ...good

This app is made as a prove of concept. There was an idea and that turned into an app; relative easily; readable and maintainable. It's used to tell students about building prototypes and starting with projects.

### ...sh*t

It breaks guidelines, many things have to be set up manually after installing the app and it uses depricated functionality. It's a bodge.

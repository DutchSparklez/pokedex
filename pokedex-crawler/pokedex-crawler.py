# Import libraries for the folowwing functionality:
from bs4 import BeautifulSoup       # Processing HTML DOM
import requests                     # Make HTML Requests
import json                         # Saving data as JSON

# The Pokemon Class: the data for a single pokemon
class Pokemon:
    # Get specific data from the provided HTML DOM.
    def get_data(self, html, css_class, isDataCollection=False):
        # Check whether we're dealing with a collection of data
        if (isDataCollection): 
            # Return the collection requested
            return list(map(lambda item: item.text, html.select(css_class)))

        # Return the element requested
        # We split the string in case there is a # in there, since this is the Pokedex number
        return html.select(css_class)[0].text.replace("\n", "").lstrip().rstrip().split('#')

    # Create an instance of the Pokemon class
    def __init__(self, data):
        # Get data that needs futher processing later
        name_and_number     = self.get_data(data, '.pokedex-pokemon-pagination-title')
        attributes          = self.get_data(data, '.attribute-value', True)

        # The actual data is saved as a dictionary because this is easier to convert to a json string
        self.data           = {
            "name"          : name_and_number[0].rstrip(),
            "number"        : name_and_number[1],
            "description"   : self.get_data(data, '.version-x')[0],
            "category"      : attributes[3],
            "imageurl"     : "https://assets.pokemon.com/assets/cms2/img/pokedex/full/%s.png" % name_and_number[1],
            "length"        : attributes[0],
            "weight"        : attributes[1],
            "abilities"     : self.get_data(data, '.active .attribute-list span', True),
            "typing"        : self.get_data(data, '.active .dtm-type a', True)
        }

# The Crawler retrieves HTML documents, prepares them for analysis and get the Pokemon data ready
class Crawler:
    # Create an instance of the class with the first URL to start scraping from. This is a partial url.
    def __init__(self, start_url):
        # Set the next URL
        self.set_next_url(start_url)

    # Prepare a new URL for scraping
    def set_next_url(self, partial_url):
        # Append the pokemon URL to the front of the partial URL, checking to make sure there isn't a dubble / after .com
        self.next_url = ('https://www.pokemon.com/' + partial_url).replace('.com//', '.com/')

    # Function that get's the data from the HTML
    def get_data(self, html):
        # Prepare the HTML for processing, prepare the next url as well.
        parsed_data = BeautifulSoup(html, 'html.parser')
        self.set_next_url(parsed_data.find('a', class_='next').get('href'))

        # Prepare the pokemon object and return it.
        return Pokemon(parsed_data)

    # Actually run the scraper
    def run(self):
        # Perpare an array that will hold all pokemon data
        pokemon = []

        # Loop over the pokemon, we only want gen 1
        for i in range(151):
            # Log to the console what's happening
            print("Collecting data on Pokémon #" + str(i + 1))

            # Start the scraping process for the current Pokemon
            pokemon.append(self.get_data(requests.get(self.next_url).text))

        # Start the conversion of the data to json, log this step to the console.
        print("Writing data to json...")
        with open("pokemon.json", 'w') as file:
            json.dump(list(map(lambda monster: monster.data, pokemon)), file)

        # We're done.
        print("Done.")

# The start of everything
if __name__ == '__main__':
    Crawler(start_url='us/pokedex/bulbasaur').run()
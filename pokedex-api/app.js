// Import express and initiate the object
const express   = require('express');
const app       = express();
const port      = 3000;

/**
 * Helper function to get a random integer between 0 and the provided maximum (included)
 * @param {number} max The maximum number to be return, included
 * @returns {number} The random integer
 */
function randomInteger(max) {
    return Math.floor(Math.random() * max + 1);
}

// Root get request :: Returns a random pokémon number
app.get("/", (_req, res) => {
    res.status(200).send((randomInteger(151)).toString());
});

// Create the server
app.listen(port, () => {
    console.log(`Pokédex API listening on port ${port}`);
});
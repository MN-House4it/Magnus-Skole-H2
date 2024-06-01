const express = require('express');
const fs = require('fs');
const path = require('path');

const app = express();
const port = 3000;
const countFilePath = path.join(__dirname, 'count.txt');

// Middleware to parse JSON bodies
app.use(express.json());
app.use(express.static(path.join(__dirname, '../client')));

// Function to read the count from the file
function getCount() {
  if (!fs.existsSync(countFilePath)) {
    fs.writeFileSync(countFilePath, '0');
  }
  const count = fs.readFileSync(countFilePath, 'utf-8');
  return parseInt(count, 10);
}

// Function to write the count to the file
function setCount(count) {
  fs.writeFileSync(countFilePath, count.toString());
}

// Route to get the current count
app.get('/count', (req, res) => {
  const count = getCount();
  res.json({ count });
});

// Route to increment the count
app.post('/increment', (req, res) => {
  let count = getCount();
  count += 1;
  setCount(count);
  res.json({ count });
});

// Route to reset the count
app.post('/reset', (req, res) => {
    count = 0;
    setCount(count);
    res.json({ count });
  });

// Start the server
app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});

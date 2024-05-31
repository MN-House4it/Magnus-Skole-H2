document.addEventListener('DOMContentLoaded', () => {
    const countDisplay = document.getElementById('count');
    const incrementButton = document.getElementById('incrementButton');
  
    // Function to fetch and display the current count
    const fetchCount = async () => {
      const response = await fetch('/count');
      const data = await response.json();
      countDisplay.innerText = data.count;
    };
  
    // Fetch the count when the page loads
    fetchCount();
  
    // Increment the count when the button is clicked
    incrementButton.addEventListener('click', async () => {
      const response = await fetch('/increment', { method: 'POST' });
      const data = await response.json();
      countDisplay.innerText = data.count;
    });
  });
  
'use client'

let count = 0;
import React, { useState } from 'react';

const Buttons = ({ startCount }) => {
  const [count, setCount] = useState(startCount);

  const handleClick = async (methode) => {
    const res = await fetch('api/count', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'countMethode' : methode
      },
    });
    if (res.ok) {
      const data = await res.json();
      setCount(data.count);
    }
  };


  return (
    <div> 
      <p>Current Count: <span id="count">{count}</span></p>
      <button onClick={() => handleClick('increase')}>Increase</button>
      <button onClick={() => handleClick('decrease')}>Decrease</button>
      <button onClick={() => handleClick('reset')}>Reset</button>
    </div>
  )
}

export default Buttons
  

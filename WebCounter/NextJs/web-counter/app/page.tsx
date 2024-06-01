import Image from "next/image";
import Buttons from "./components/ClientSide";


let clickCount = 0;
const increaseCount = () => {
  clickCount += 1;
};
const decreaseCount = () => {
  clickCount -= 1;
};
const resetCount = () => {
  clickCount = 0;
};
export { clickCount, increaseCount, decreaseCount, resetCount };


export default function Home() {


  
  return (
    <main>
      <div className="container">
      <h1>Counter App Next Js</h1>      
      <Buttons startCount={clickCount}/>
    </div>
      

      
    </main>
  );
}

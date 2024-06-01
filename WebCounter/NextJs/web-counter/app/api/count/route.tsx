import { NextResponse } from "next/server";
// route.tsx
import { clickCount, increaseCount, decreaseCount, resetCount} from '../../page';


export async function GET(request: Request) {
  // ...
  
  return NextResponse.json({ message: clickCount });
}

// Handles POST requests to /api
export async function POST(request: Request) {
  const headerValue = request.headers.get('countMethode');
  if (headerValue === 'increase') {
    increaseCount();
    return NextResponse.json({ count: clickCount });
  } else if (headerValue === 'decrease') {
    decreaseCount();
    return NextResponse.json({ count: clickCount });
  } else if (headerValue === 'reset') {
    resetCount();
    return NextResponse.json({ count: clickCount });
  } else {
    return NextResponse.json('Missing headder with countMethode');
  }
  
}
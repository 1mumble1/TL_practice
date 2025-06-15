import './App.css';
import { type Review } from './types/Review';
import { useState } from 'react';
import ReviewForm from './components/ReviewForm/ReviewForm';

function App() {
  const [reviews, setReviews] = useState<Review[]>([]);

  const addReview = (review: Review) => {
    const updatedReviews = [...reviews, review];
    setReviews(updatedReviews);
  }

  return (
    <div className='content'>
      <ReviewForm onAddReview={addReview} />
      <Reviews reviews={reviews} />
    </div>
  );
}

export default App;
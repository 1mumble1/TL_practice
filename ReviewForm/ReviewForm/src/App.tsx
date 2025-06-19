import './App.css';
import { type Review } from './types/types';
import { useState } from 'react';
import ReviewForm from './components/ReviewForm/ReviewForm';
import Reviews from './components/Reviews/Reviews';

function App() {
  const [reviews, setReviews] = useState<Review[]>([]);

  const handleSumbitForm = ({name, comment, rating}: Review) => {
    setReviews([...reviews, {name , comment, rating}]);
  }

  return (
    <>
      <ReviewForm onSubmit={handleSumbitForm}/>
      {reviews && (<Reviews reviews={reviews} />)}
    </>
  );
}

export default App;
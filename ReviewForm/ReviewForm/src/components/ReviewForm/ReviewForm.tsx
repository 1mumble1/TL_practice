import type { Review } from "../../types/Review";

interface ReviewFormProps {
  onAddReview: (review: Review) => void;
}

const ReviewForm = (props: ReviewFormProps) => {
  const addReview = props.onAddReview;

  return (
    <>
      
    </>
  );
}

export default ReviewForm;
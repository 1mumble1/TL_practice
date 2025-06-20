import { useState } from "react";
import type { Review } from "../../types/types";
import styles from './ReviewForm.module.css'
import Rating from "../Rating/Rating";
import SubmitButton from "../SubmitButton/SubmitButton";
import NameInput from "../NameInput/NameInput";
import CommentArea from "../CommentArea/CommentArea";

interface ReviewFormProps {
  onSubmit: (review: Review) => void;
}

const ReviewForm: React.FC<ReviewFormProps> = ({ onSubmit }) => {

  const [rating, setRating] = useState(0);
  const [name, setName] = useState('');
  const [comment, setComment] = useState('');

  const isFormValid = name.trim() !== '' && comment.trim() !== '' && rating > 0

  const handleSumbit = (event: React.FormEvent) => {
    event.preventDefault();
    
    if (isFormValid) {
      onSubmit({
        name: name.trim(),
        comment: comment.trim(),
        rating,
      });
    }

    setName('');
    setComment('');
    setRating(0);
  };

  return (
    <form className={styles.form} onSubmit={handleSumbit}>
      <h1 className={styles.title}>
        Помогите нам сделать процесс бронирования лучше
      </h1>
      <Rating
        pressedButton={rating}
        onPressButton={setRating}
      />
      <NameInput name={name} onChange={setName} />
      <CommentArea comment={comment} onChange={setComment} />
      <SubmitButton isDisabled={!isFormValid} />
    </form>
  );
};

export default ReviewForm;
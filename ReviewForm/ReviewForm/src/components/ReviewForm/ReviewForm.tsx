import { useState } from "react";
import type { Review } from "../../types/types";
import styles from './ReviewForm.module.css'
import Rating from "../Rating/Rating";
import SubmitButton from "../SubmitButton/SubmitButton";

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
  }

  return (
    <form className={styles.form} onSubmit={handleSumbit}>
      <h1 className={styles.title}>
        Помогите нам сделать процесс бронирования лучше
      </h1>
      <Rating
        pressedButton={rating}
        onPressButton={setRating}
      />
      <div className={styles.nameContainer}>
        <label htmlFor="name">*Имя</label>
        <input
          type="text"
          id="name"
          placeholder="Как вас зовут?"
          value={name}
          onChange={(event) => setName(event.target.value)}
        />
      </div>
      <div className={styles.commentContainer}>
        <textarea
          id="comment"
          placeholder="Напишите, что понравилось, что было непонятно"
          value={comment}
          onChange={(event) => setComment(event.target.value)}
        ></textarea>
      </div>
      <div className={styles.buttonContainer}>
        <SubmitButton isDisabled={!isFormValid} />
      </div>
    </form>
  )
}

export default ReviewForm;
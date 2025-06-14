import React, { useState } from 'react';
import RatingSection from '../RatingSection/RatingSection';
import CommentSection from '../CommentSection/CommentSection';
import NameInput from '../NameInput/NameInput';
import SubmitButton from '../SubmitButton/SubmitButton';
import styles from './FeedbackForm.module.css';

export interface FeedbackData {
  cleanliness: number;
  service: number;
  speed: number;
  location: number;
  speechCulture: number;
  comment: string;
  name: string;
}

const FeedbackForm: React.FC = () => {
  const [feedback, setFeedback] = useState<FeedbackData>({
    cleanliness: 0,
    service: 0,
    speed: 0,
    location: 0,
    speechCulture: 0,
    comment: '',
    name: '',
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log('Feedback submitted:', feedback);
    // Здесь можно добавить логику отправки данных
  };

  return (
    <form className={styles.form} onSubmit={handleSubmit}>
      <h1 className={styles.title}>Помогите нам сделать процесс бронирования лучше</h1>
      <RatingSection />
      <CommentSection />
    </form>
  );
};

export default FeedbackForm;
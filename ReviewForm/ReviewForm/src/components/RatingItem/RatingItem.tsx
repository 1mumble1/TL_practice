// RatingItem.tsx
import React from 'react';
import styles from '../RatingSection/RatingSection.module.css';

interface RatingItemProps {
  label: string;
  value: number;
  onChange: (value: number) => void;
}

const RatingItem: React.FC<RatingItemProps> = ({ label, value, onChange }) => {
  return (
    <div className={styles.ratingItem}>
      <span className={styles.ratingLabel}>{label}</span>
      <div className={styles.ratingDots}>
        {[1, 2, 3, 4, 5].map((star) => (
          <button
            key={star}
            type="button"
            className={`${styles.dot} ${value >= star ? styles.filled : ''}`}
            onClick={() => onChange(star)}
            aria-label={`Оценка ${star}`}
          />
        ))}
      </div>
    </div>
  );
};

export default RatingItem;
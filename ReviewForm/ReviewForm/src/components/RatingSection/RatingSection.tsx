// RatingSection.tsx
import React from 'react';
import RatingItem from '../RatingItem/RatingItem';
import styles from './RatingSection.module.css';

interface RatingSectionProps {
  ratings: {
    category: string;
    label: string;
  }[];
  values: Record<string, number>;
  onChange: (category: string, value: number) => void;
}

const RatingSection: React.FC<RatingSectionProps> = ({ ratings, values, onChange }) => {
  return (
    <div className={styles.ratingSection}>
      {ratings.map((rating) => (
        <RatingItem
          key={rating.category}
          label={rating.label}
          value={values[rating.category as keyof typeof values]}
          onChange={(value) => onChange(rating.category, value)}
        />
      ))}
    </div>
  );
};

export default RatingSection;
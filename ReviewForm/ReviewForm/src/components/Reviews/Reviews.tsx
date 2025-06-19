import type React from "react";
import type { Review } from "../../types/types";
import styles from './Reviews.module.css';
import Avatar from '../../../public/twemoji_angry-face.png'

interface ReviewsProps {
    reviews: Review[];
}

const Reviews: React.FC<ReviewsProps> = ({ reviews }) => {

  return (
    <>
      {reviews.map((review, index) => {
        return (
          <div key={index} className={styles.container}>
            <div className={styles.avatarContainer}>
              <img
                className={styles.avatar}
                src={Avatar}
                alt="avatar"
              />
            </div>
            <div className={styles.ratingContainer}>
              <div className={styles.header}>
                <p className={styles.name}>{review.name}</p>
                <p className={styles.rating}>{review.rating + '/5'}</p>
              </div>
              <p className={styles.comment}>{review.comment}</p>
            </div>
          </div>
        );
      })}
    </>
  );     
}

export default Reviews;
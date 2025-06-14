import React from 'react';
import styles from './CommentSection.module.css';

interface CommentSectionProps {
  value: string;
  onChange: (value: string) => void;
  placeholder: string;
}

const CommentSection = () => {
  return (
    <div className={styles.commentSection}>
      <textarea
        placeholder="Напишите что нибудь"
        className={styles.textarea}
        rows={6}
      />
    </div>
  );
};

export default CommentSection;
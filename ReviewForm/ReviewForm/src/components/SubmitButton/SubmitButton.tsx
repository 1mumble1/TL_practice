import React from 'react';
import styles from './SubmitButton.module.css';

const SubmitButton: React.FC = () => {
  return (
    <div className={styles.buttonContainer}>
      <button type="submit" className={styles.button}>
        Отправить
      </button>
    </div>
  );
};

export default SubmitButton;
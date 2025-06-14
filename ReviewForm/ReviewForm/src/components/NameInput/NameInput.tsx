import React from 'react';
import styles from './NameInput.module.css';

interface NameInputProps {
  value: string;
  onChange: (value: string) => void;
  placeholder: string;
}

const NameInput: React.FC<NameInputProps> = ({ value, onChange, placeholder }) => {
  return (
    <div className={styles.nameInputContainer}>
      <label className={styles.label}>
        <span className={styles.required}>*</span>Имя
      </label>
      <input
        type="text"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder={placeholder}
        className={styles.input}
        required
      />
    </div>
  );
};

export default NameInput;
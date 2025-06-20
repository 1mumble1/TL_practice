import styles from './NameInput.module.css';

interface NameInputProps {
  name: string;
  onChange: (value: string) => void;
};

const NameInput: React.FC<NameInputProps> = ({ name, onChange }) => {

  return (
    <div className={styles.nameContainer}>
      <label htmlFor="name">*Имя</label>
      <input
        type="text"
        id="name"
        placeholder="Как вас зовут?"
        value={name}
        onChange={(event) => onChange(event.target.value)}
      />
    </div>
  );
};

export default NameInput;
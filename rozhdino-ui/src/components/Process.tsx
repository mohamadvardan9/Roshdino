{
  /*شروع از بخش مسیر همکاری با رشدینو*/
}
{
  /*این بخش روند پیش روی کار و سیاست های سایت را توضیح میدههد*/
}

import Container from "./ui/Container";
import { motion } from "framer-motion";

export default function Process() {
  const steps = [
    {
      number: "1",
      title: "مشاوره اولیه",
      description:
        "در اولین مرحله نیازها و اهداف کسب‌وکار شما را بررسی می‌کنیم.",
    },
    {
      number: "2",
      title: "تحلیل کسب‌وکار",
      description: "بازار، رقبا و مسیر مناسب رشد را تحلیل می‌کنیم.",
    },
    {
      number: "3",
      title: "طراحی استراتژی",
      description: "یک برنامه دقیق برای رسیدن به اهداف شما طراحی می‌کنیم.",
    },
    {
      number: "4",
      title: "اجرا و توسعه",
      description: "راهکارهای دیجیتال را اجرا کرده و نتایج را بررسی می‌کنیم.",
    },
  ];

  return (
    <section
      className="
        py-24
        bg-gray-50
        "
    >
      <Container>
        <div
          className="
                text-center
                "
        >
          <h2
            className="
                    text-4xl
                    font-black
                    "
          >
            مسیر همکاری با رشدینو
          </h2>

          <p
            className="
                    mt-5
                    text-gray-600
                    "
          >
            از ایده تا رشد، قدم به قدم کنار شما هستیم
          </p>
        </div>

        <div
          className="
                mt-16
                grid
                md:grid-cols-4
                gap-8
                "
        >
          {steps.map((step) => (
            <div
              key={step.number}
              className="
                        bg-white
                        rounded-3xl
                        p-8
                        border
                        hover:shadow-xl
                        transition
                        "
            >
              <motion.div
                initial={{ scale: 0, opacity: 0 }}
                whileInView={{ scale: 1, opacity: 1 }}
                viewport={{ once: true }}
                transition={{
                  duration: 0.5,
                  type: "spring",
                  stiffness: 200,
                }}
                whileHover={{
                  scale: 1.1,
                  rotate: 5,
                }}
                className="
                                w-16
                                h-16
                                rounded-full
                                bg-gradient-to-br
                                from-purple-600
                                to-green-400
                                flex
                                items-center
                                justify-center
                                text-white
                                text-2xl
                                font-black
                                shadow-lg
                                "
              >
                {step.number}
              </motion.div>

              <h3
                className="
                            mt-6
                            text-xl
                            font-bold
                            "
              >
                {step.title}
              </h3>

              <p
                className="
                            mt-4
                            text-gray-500
                            leading-7
                            "
              >
                {step.description}
              </p>
            </div>
          ))}
        </div>
      </Container>
    </section>
  );
}
